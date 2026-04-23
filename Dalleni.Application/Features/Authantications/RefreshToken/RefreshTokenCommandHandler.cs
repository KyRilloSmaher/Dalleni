using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Dalleni.Application.Features.Authantications.RefreshToken
{
    internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenAsyncCommand, Response<TokenReponseDto>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, ITokenService tokenService, ILogger<RefreshTokenCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Response<TokenReponseDto>> Handle(RefreshTokenAsyncCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            var principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
            var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.INVALID_TOKEN);
            }

            var user = await _unitOfWork.UserManager.FindByIdAsync(userId, true);
            if (user is null)
            {
                return _responseHandler.Unauthorized<TokenReponseDto>(SystemMessages.USER_NOT_FOUND);
            }

            if (user.RefreshToken != dto.RefreshToken)
            {
                return _responseHandler.Unauthorized<TokenReponseDto>(SystemMessages.INVALID_REFRESH_TOKEN);
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return _responseHandler.Unauthorized<TokenReponseDto>(SystemMessages.REFRESH_TOKEN_EXPIRED);
            }

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = _tokenService.GetRefreshTokenExpiryTime();
            var updateResult = await _unitOfWork.UserManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to rotate refresh token for {UserId}: {Errors}", userId, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.FAILED);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var claims = await _tokenService.GetClaimsAsync(user);
            return _responseHandler.Success(new TokenReponseDto
            {
                AccessToken = _tokenService.GenerateAccessToken(claims),
                RefreshToken = user.RefreshToken,
                AccessTokenExpiresAt = DateTime.UtcNow.AddDays(7),
                RefreshTokenExpiresAt = user.RefreshTokenExpiryTime!.Value,
            }, SystemMessages.TOKEN_REFRESHED_SUCCESS);
        }
    }
}
