using Dalleni.Domin.Enums;

namespace Dalleni.Application.Services.Notifications
{
    public interface INotificationService
    {
        Task CreateAsync(
            Guid recipientId,
            Guid? actorId,
            NotificationType type,
            NotificationEntityType entityType,
            Guid? entityId,
            NotificationChannel channels,
            string title,
            string message,
            CancellationToken cancellationToken = default
            );
    }
}