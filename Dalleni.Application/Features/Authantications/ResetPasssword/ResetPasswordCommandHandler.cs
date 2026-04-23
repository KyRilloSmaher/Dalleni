using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Authantications.ResetPasssword
{
    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, ILogger<ResetPasswordCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.dto;
            var user = await _unitOfWork.UserManager.FindByEmailAsync(dto.Email, true);
            if (user is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USER_NOT_FOUND);
            }

            if (!user.ResetPasswordConfirmed)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.RESET_NOT_CONFIRMED);
            }

            var removePassResult = await _unitOfWork.UserManager.RemovePasswordAsync(user);
            if (!removePassResult.Succeeded)
            {
                _logger.LogError("Failed to remove password for {Email}: {Errors}", dto.Email, string.Join(", ", removePassResult.Errors.Select(e => e.Description)));
                return _responseHandler.BadRequest<bool>(SystemMessages.FAILED);
            }

            var addPassResult = await _unitOfWork.UserManager.AddPasswordAsync(user, dto.NewPassword);
            if (!addPassResult.Succeeded)
            {
                _logger.LogError("Failed to add password for {Email}: {Errors}", dto.Email, string.Join(", ", addPassResult.Errors.Select(e => e.Description)));
                return _responseHandler.BadRequest<bool>(SystemMessages.FAILED);
            }

            await _unitOfWork.UserManager.UpdateSecurityStampAsync(user);
            user.ResetPasswordConfirmed = false;
            await _unitOfWork.UserManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _responseHandler.Success(true, SystemMessages.PASSWORD_RESET_SUCCESS);
        }
    }
}
