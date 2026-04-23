using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Authantications.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserManager.FindByIdAsync(request.userId, true);
            if (user is null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.USER_NOT_FOUND);
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _unitOfWork.UserManager.UpdateSecurityStampAsync(user);
            var updateResult = await _unitOfWork.UserManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return _responseHandler.BadRequest<bool>(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _responseHandler.Success(true, SystemMessages.LOGOUT_SUCCESS);
        }
    }
}
