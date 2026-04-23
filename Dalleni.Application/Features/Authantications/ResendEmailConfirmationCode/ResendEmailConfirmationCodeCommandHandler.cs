using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Dalleni.Application.Features.Authantications.ResendEmailConfirmationCode
{
    internal class ResendEmailConfirmationCodeCommandHandler : IRequestHandler<ResendConfirmaionEmailCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<ResendEmailConfirmationCodeCommandHandler> _logger;

        public ResendEmailConfirmationCodeCommandHandler(
            IResponseHandler responseHandler,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            ILogger<ResendEmailConfirmationCodeCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(ResendConfirmaionEmailCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            var user = await _unitOfWork.UserManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USER_NOT_FOUND);
            }

            if (user.EmailConfirmed)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.EMAIL_ALREADY_VERIFIED);
            }

            var existingVerification = await _unitOfWork.OtpCodes.GetLatestActiveAsync(user.Id, OtpType.EmailVerification, false, cancellationToken);
            var token = existingVerification?.Code ?? RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");

            if (existingVerification is null)
            {
                await _unitOfWork.OtpCodes.AddAsync(OtpCode.Create(token, OtpType.EmailVerification, DateTime.UtcNow.AddHours(24), user.Id), cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var emailSent = await _emailService.SendEmailAsync(user.Email, "Confirm Email", $"Use this OTP to confirm your email: {token}");
            if (!emailSent)
            {
                _logger.LogError("Failed to resend confirmation email to {Email}", dto.Email);
                return _responseHandler.BadRequest<bool>(SystemMessages.FAILED);
            }

            return _responseHandler.Success(true, SystemMessages.EMAIL_SENT);
        }
    }
}
