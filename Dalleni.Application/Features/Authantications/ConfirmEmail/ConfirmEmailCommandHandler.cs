using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Authantications.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(
            IResponseHandler responseHandler,
            IUnitOfWork unitOfWork,
            ILogger<ConfirmEmailCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            var user = await _unitOfWork.UserManager.FindByEmailAsync(dto.Email, true);
            if (user is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USER_NOT_FOUND);
            }

            if (user.EmailConfirmed)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.EMAIL_ALREADY_VERIFIED);
            }

            var verification = await _unitOfWork.OtpCodes.GetByCodeAsync(user.Id, dto.Code, OtpType.EmailVerification, true, cancellationToken);
            if (verification is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.INVALID_TOKEN);
            }

            try
            {
                user.EmailConfirmed = true;
                verification.MarkAsUsed();
                await _unitOfWork.UserManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return _responseHandler.Success(true, SystemMessages.VERIFICATION_SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming email for {Email}", dto.Email);
                return _responseHandler.BadRequest<bool>(SystemMessages.VERIFICATION_FAILED);
            }
        }
    }
}
