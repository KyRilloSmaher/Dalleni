using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Notifications.Commands.MarkAsRead
{
    /// <summary>
    /// Marks a notification belonging to the authenticated user as read.
    /// </summary>
    public record MarkNotificationAsReadCommand(Guid UserId, Guid NotificationId) : IRequest<Response<bool>>;
}