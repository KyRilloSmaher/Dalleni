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

namespace Dalleni.Application.Features.Authantications.SignUp
{
    internal class SignUpCommandHandler : IRequestHandler<SignUpCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly ILogger<SignUpCommandHandler> _logger;

        public SignUpCommandHandler(
            IResponseHandler responseHandler,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IImageUploaderService imageUploaderService,
            ILogger<SignUpCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _imageUploaderService = imageUploaderService;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            string? uploadedImageUrl = null;
            var dto = request.dto;
            var fullName = $"{dto.FirstName} {dto.LastName}".Trim();

            var emailExists = await _unitOfWork.UserManager.FindByEmailAsync(dto.Email);
            if (emailExists is not null)
            {
                if(emailExists.IsDeleted && !string.IsNullOrEmpty(emailExists.PhoneNumber))
                {
                    return _responseHandler.BadRequest<bool>(SystemMessages.YOU_CAN_RESTORE_YOUR_ACCOUNT);
                }
                return _responseHandler.BadRequest<bool>(SystemMessages.EMAIL_ALREADY_EXISTS);
            }

            if (await _unitOfWork.UserManager.FindByNameAsync(dto.UserName) is not null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USERNAME_ALREADY_EXISTS);
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && (await _unitOfWork.Users.GetAllAsync(false, cancellationToken)).OfType<ApplicationUser>().Any(x => x.PhoneNumber == dto.PhoneNumber))
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.PHONE_ALREADY_EXISTS);
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (dto.ProfileImage is not null)
                {
                    var uploadResult = await _imageUploaderService.UploadImageAsync(dto.ProfileImage, ImageFolder.profiles);
                    if (uploadResult.Error is not null)
                    {
                        await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                        return _responseHandler.BadRequest<bool>(SystemMessages.FAILED);
                    }

                    uploadedImageUrl = uploadResult.Url.ToString();
                }

                var user = ApplicationUser.Create(fullName, dto.Email, dto.UserName, dto.PhoneNumber, uploadedImageUrl);
                var createResult = await _unitOfWork.UserManager.CreateAsync(user, dto.Password);
                if (!createResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    if (!string.IsNullOrWhiteSpace(uploadedImageUrl))
                    {
                        await _imageUploaderService.DeleteImageByUrlAsync(uploadedImageUrl);
                    }

                    return _responseHandler.BadRequest<bool>(string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }

                var roleResult = await _unitOfWork.UserManager.AddToRoleAsync(user, "USER");
                if (!roleResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return _responseHandler.BadRequest<bool>(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                var token = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
                var otpRecord = OtpCode.Create(token, OtpType.EmailVerification, DateTime.UtcNow.AddHours(24), user.Id);
                await _unitOfWork.OtpCodes.AddAsync(otpRecord, cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var emailSent = await _emailService.SendEmailAsync(user.Email, "Confirm Email", $"Use this OTP to confirm your email: {token}");
                if (!emailSent)
                {
                    _logger.LogWarning("User created but confirmation email failed for {Email}", user.Email);
                }

                return _responseHandler.Success(true, SystemMessages.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during signup for {Email}", dto.Email);
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                if (!string.IsNullOrWhiteSpace(uploadedImageUrl))
                {
                    await _imageUploaderService.DeleteImageByUrlAsync(uploadedImageUrl);
                }

                return _responseHandler.BadRequest<bool>(SystemMessages.FAILED);
            }
        }
    }
}
