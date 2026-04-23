using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Authantications.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, ILogger<ChangePasswordCommandHandler> logger)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(request.UserId, true);
            if (user is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USER_NOT_FOUND);
            }

            var changeResult = await _unitOfWork.UserManager.ChangePasswordAsync(user, request.dto.CurrentPassword, request.dto.NewPassword);
            if (!changeResult.Succeeded)
            {
                _logger.LogError("Failed to change password for {UserId}: {Errors}", request.UserId, string.Join(", ", changeResult.Errors.Select(e => e.Description)));
                return _responseHandler.BadRequest<bool>(string.Join(", ", changeResult.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.UserManager.UpdateSecurityStampAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _responseHandler.Success(true, SystemMessages.PASSWORD_CHANGED_SUCCESS);
        }
    }
}
