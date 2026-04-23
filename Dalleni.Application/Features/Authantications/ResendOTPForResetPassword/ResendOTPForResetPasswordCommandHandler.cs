using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Authantications.ResendOTPForResetPassword
{
    internal class ResendOTPForResetPasswordCommandHandler : IRequestHandler<ReSendOTPForResetPasswordCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<ResendOTPForResetPasswordCommandHandler> _logger;
        private readonly PasswordHasher<Dalleni.Domin.Models.ApplicationUser> _passwordHasher = new();

        public ResendOTPForResetPasswordCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, IEmailService emailService, ILogger<ResendOTPForResetPasswordCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(ReSendOTPForResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            var user = await _unitOfWork.UserManager.FindByEmailAsync(dto.Email, true);
            if (user is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USER_NOT_FOUND);
            }

            if (user.OTPExpiryTime.HasValue && user.OTPExpiryTime.Value > DateTime.UtcNow.AddMinutes(-2))
            {
                var secondsRemaining = (int)(user.OTPExpiryTime.Value - DateTime.UtcNow).TotalSeconds;
                return _responseHandler.BadRequest<bool>(string.Format(SystemMessages.PASSWORD_RESET_RATE_LIMIT, secondsRemaining));
            }

            var otp = new Random().Next(0, 1_000_000).ToString("D6");
            user.OTP = _passwordHasher.HashPassword(user, otp);
            user.OTPExpiryTime = DateTime.UtcNow.AddMinutes(15);
            user.OTPAttempts = 0;
            user.ResetPasswordConfirmed = false;

            var updateResult = await _unitOfWork.UserManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to update reset OTP for {Email}: {Errors}", dto.Email, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                return _responseHandler.BadRequest<bool>(SystemMessages.FAILED);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _emailService.SendPasswordResetEmailAsync(user.Email, "Reset Password", otp);
            return _responseHandler.Success(true, SystemMessages.RESET_PASSWORD_CODE_SENT);
        }
    }
}
