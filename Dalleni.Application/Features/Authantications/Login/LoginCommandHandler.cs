using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Domin.Settings;
using MediatR;

namespace Dalleni.Application.Features.Authantications.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<TokenReponseDto>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, ITokenService tokenService, JwtSettings jwtSettings)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
        }

        public async Task<Response<TokenReponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            var user = await _unitOfWork.UserManager.FindByEmailAsync(dto.Email, true);

            if (user == null || !await _unitOfWork.UserManager.CheckPasswordAsync(user, dto.Password))
            {
                if (user != null)
                {
                    await _unitOfWork.UserManager.AccessFailedAsync(user);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                return _responseHandler.Unauthorized<TokenReponseDto>(SystemMessages.INVALID_CREDENTIALS);
            }

            if (!user.EmailConfirmed)
            {
                return _responseHandler.Unauthorized<TokenReponseDto>(SystemMessages.EMAIL_NOT_CONFIRMED);
            }

            if (await _unitOfWork.UserManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _unitOfWork.UserManager.GetLockoutEndDateAsync(user);
                var minutesRemaining = (lockoutEnd - DateTimeOffset.UtcNow)?.Minutes ?? 0;
                return _responseHandler.Unauthorized<TokenReponseDto>(string.Format(SystemMessages.ACCOUNT_LOCKED, minutesRemaining));
            }

            user.RecordLogin();
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = _tokenService.GetRefreshTokenExpiryTime();
            await _unitOfWork.UserManager.ResetAccessFailedCountAsync(user);
            await _unitOfWork.UserManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
    }
}
