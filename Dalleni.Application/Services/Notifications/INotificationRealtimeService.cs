using Dalleni.Application.DTOs.Responses.Notifications;

namespace Dalleni.Application.Services.Notifications
{
    public interface INotificationRealtimeService
    {
        Task SendAsync(
            Guid recipientId,
            NotificationResponseDto notification,
            CancellationToken cancellationToken = default);
    }
}