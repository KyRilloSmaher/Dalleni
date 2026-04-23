using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Domin.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Dalleni.Application.Features.Authantications.GooleSignin
{
    internal class GooleSigninCommandHandler : IRequestHandler<GooleSigninCommand, Response<TokenReponseDto>>
    {
        private const string GoogleProvider = "Google";
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<GooleSigninCommandHandler> _logger;

        public GooleSigninCommandHandler(
            IResponseHandler responseHandler,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            JwtSettings jwtSettings,
            ILogger<GooleSigninCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public async Task<Response<TokenReponseDto>> Handle(GooleSigninCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.email) || string.IsNullOrWhiteSpace(request.nameIdentifier))
            {
                return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.INVALID_INPUT);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var externalLogin = await _unitOfWork.ExternalLogins.GetByProviderAsync(
                    GoogleProvider,
                    request.nameIdentifier,
                    asTracked: true,
                    cancellationToken: cancellationToken);

                ApplicationUser? user = null;

                if (externalLogin is not null)
                {
                    user = await _unitOfWork.UserManager.FindByIdAsync(externalLogin.UserId, true);
                    if (user is null)
                    {
                        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return _responseHandler.Unauthorized<TokenReponseDto>(SystemMessages.USER_NOT_FOUND);
                    }
                }
                else
                {
                    user = await _unitOfWork.UserManager.FindByEmailAsync(request.email, true);
                    if (user is null)
                    {
                        user = await CreateGoogleUserAsync(request);
                        if (user is null)
                        {
                            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                            return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.FAILED);
                        }
                    }

                    externalLogin = ExternalLogin.Create(
                        GoogleProvider,
                        request.nameIdentifier,
                        user.Id,
                        request.email,
                        BuildDisplayName(request),
                        request.picture);

                    await _unitOfWork.ExternalLogins.AddAsync(externalLogin, cancellationToken);
                }

                externalLogin.UpdateProfile(request.email, BuildDisplayName(request), request.picture);
                externalLogin.RecordUsage();

                if (string.IsNullOrWhiteSpace(user.ProfileImageUrl) && !string.IsNullOrWhiteSpace(request.picture))
                {
                    user.UpdateProfile(user.FullName, request.picture);
                }

                user.EmailConfirmed = true;
                user.RecordLogin();
                user.RefreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshTokenExpiryTime = _tokenService.GetRefreshTokenExpiryTime();

                var updateResult = await _unitOfWork.UserManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _logger.LogError("Failed to update google signin user {Email}: {Errors}", request.email, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return _responseHandler.BadRequest<TokenReponseDto>(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                }

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var claims = await _tokenService.GetClaimsAsync(user);
                var accessToken = _tokenService.GenerateAccessToken(claims);

                return _responseHandler.Success(new TokenReponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = user.RefreshToken,
                    AccessTokenExpiresAt = DateTime.UtcNow.AddHours(_jwtSettings.AccessTokenLifetimeHours),
                    RefreshTokenExpiresAt = user.RefreshTokenExpiryTime!.Value,
                }, SystemMessages.LOGIN_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during google signin for {Email}", request.email);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.FAILED);
            }
        }

        private async Task<ApplicationUser?> CreateGoogleUserAsync(GooleSigninCommand request)
        {
            var fullName = BuildDisplayName(request);
            var username = await GenerateUniqueUserNameAsync(request.Username, request.email);
            var user = ApplicationUser.Create(fullName, request.email, username, profileImageUrl: request.picture);
            user.EmailConfirmed = true;

            var createResult = await _unitOfWork.UserManager.CreateAsync(user, GenerateTemporaryPassword());
            if (!createResult.Succeeded)
            {
                _logger.LogError("Failed to create google user {Email}: {Errors}", request.email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                return null;
            }

            var roleResult = await _unitOfWork.UserManager.AddToRoleAsync(user, "USER");
            if (!roleResult.Succeeded)
            {
                _logger.LogError("Failed to assign USER role to google user {Email}: {Errors}", request.email, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                return null;
            }

            return user;
        }

        private async Task<string> GenerateUniqueUserNameAsync(string? requestedUserName, string email)
        {
            var baseName = SanitizeUserName(requestedUserName);
            if (string.IsNullOrWhiteSpace(baseName))
            {
                baseName = SanitizeUserName(email.Split('@')[0]);
            }

            if (string.IsNullOrWhiteSpace(baseName))
            {
                baseName = $"user{Guid.NewGuid():N}"[..12];
            }

            var candidate = baseName;
            var suffix = 1;
            while (await _unitOfWork.UserManager.FindByNameAsync(candidate) is not null)
            {
                candidate = $"{baseName}{suffix++}";
            }

            return candidate;
        }

        private static string BuildDisplayName(GooleSigninCommand request)
        {
            var fullName = $"{request.Username} {request.Surname}".Trim();
            return string.IsNullOrWhiteSpace(fullName) ? request.email : fullName;
        }

        private static string SanitizeUserName(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var sanitized = Regex.Replace(value.Trim(), @"[^a-zA-Z0-9._]", string.Empty);
            return sanitized.Length <= 50 ? sanitized : sanitized[..50];
        }

        private static string GenerateTemporaryPassword()
            => $"Gg!{Guid.NewGuid():N}1aA";
    }
}
