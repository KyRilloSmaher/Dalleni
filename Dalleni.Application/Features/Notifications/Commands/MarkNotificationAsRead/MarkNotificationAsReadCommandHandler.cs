using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Notifications.Commands.MarkAsRead
{
    /// <summary>
    /// Handles marking a notification as read.
    /// </summary>
    public class MarkNotificationAsReadCommandHandler: IRequestHandler<MarkNotificationAsReadCommand,Response<bool>>
    {
       private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;

        public MarkNotificationAsReadCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }

        public async Task<Response<bool>> Handle( MarkNotificationAsReadCommand request,CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var notification = await _unitOfWork.Notifications.GetByIdAsync(request.NotificationId, asTracked: true, cancellationToken: cancellationToken);

            if (notification is null)
            {
                return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
            }

            if (notification.RecipientId != userId)
            {
                return _responseHandler.Forbidden<bool>(SystemMessages.UNAUTHORIZED);
            }

            notification.MarkAsRead();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _responseHandler.Success(true, SystemMessages.RECORD_UPDATED);
        }
    }
}