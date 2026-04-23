using AutoMapper;
using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Application.Features.Users.Commands.DeleteAccount;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Interfaces.Services;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Domin.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Commands.RestoreAccount
{
    public class RestoreAccountCommandHandler : IRequestHandler<RestoreAccountCommand, Response<TokenReponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;
        private readonly ILogger<RestoreAccountCommandHandler> _logger;

        public RestoreAccountCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper, ILogger<RestoreAccountCommandHandler> logger, ITokenService tokenService, JwtSettings jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings;
        }

        public async Task<Response<TokenReponseDto>> Handle(RestoreAccountCommand command, CancellationToken cancellationToken)
        {
            var request = command.request;
            if (request == null)
            {
                _logger.LogError("RestoreAccountCommand is null.");
                return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.FAILED);

            }
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, true);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", request.Email);
                return _responseHandler.NotFound<TokenReponseDto>(SystemMessages.NOT_FOUND);
            }

            if (!user.IsDeleted)
            {
                _logger.LogInformation("User with email {Email} is not deleted.", request.Email);
                return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.FAILED);
            }
            if (user.PhoneNumber != null && user.PhoneNumber != request.PhoneNumber){
             
                _logger.LogWarning("Phone number mismatch for user with email {Email}.", request.Email);
                 return _responseHandler.BadRequest<TokenReponseDto>(SystemMessages.FAILED);
            }
            user.Restore();
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
