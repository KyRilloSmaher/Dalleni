using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Authantications.ConfirmOTPForResetPassword
{
    internal class ConfirmOTPForResetPasswordCommandHandler : IRequestHandler<ConfirmResetPasswordOTPCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConfirmOTPForResetPasswordCommandHandler> _logger;
        private readonly PasswordHasher<Dalleni.Domin.Models.ApplicationUser> _passwordHasher = new();

        public ConfirmOTPForResetPasswordCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, ILogger<ConfirmOTPForResetPasswordCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(ConfirmResetPasswordOTPCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            var user = await _unitOfWork.UserManager.FindByEmailAsync(dto.Email, true);
            if (user is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USER_NOT_FOUND);
            }

            if (string.IsNullOrEmpty(user.OTP))
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.NO_RESET_CODE);
            }

            if (user.OTPExpiryTime < DateTime.UtcNow)
            {
                user.OTP = null;
                user.OTPExpiryTime = null;
                user.OTPAttempts = 0;
                await _unitOfWork.UserManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _responseHandler.BadRequest<bool>(SystemMessages.RESET_CODE_EXPIRED);
            }

            if (user.OTPAttempts >= 5)
            {
                user.OTP = null;
                user.OTPExpiryTime = null;
                user.OTPAttempts = 0;
                await _unitOfWork.UserManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _responseHandler.BadRequest<bool>(SystemMessages.MAX_ATTEMPTS_REACHED);
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.OTP, dto.Code);
            if (verificationResult == PasswordVerificationResult.Failed)
            {
                user.OTPAttempts++;
                await _unitOfWork.UserManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _responseHandler.BadRequest<bool>(SystemMessages.INVALID_RESET_CODE);
            }

            user.ResetPasswordConfirmed = true;
            user.OTP = null;
            user.OTPExpiryTime = null;
            user.OTPAttempts = 0;
            await _unitOfWork.UserManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _responseHandler.Success(true, SystemMessages.PASSWORD_RESET_CODE_CONFIRMED);
        }
    }
}
