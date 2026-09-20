using Dalleni.Application.DTOs.Notifications;

namespace Dalleni.Application.Services.Notifications
{
    public interface INotificationPushService
    {
        Task SendAsync(
            Guid recipientId,
            NotificationPushPayloadDto payload,
            CancellationToken cancellationToken = default);
    }
}