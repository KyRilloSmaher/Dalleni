using Dalleni.Application.Services.BackgroundJobs;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IBackgroundJobService _backgroundJobService;

        public NotificationService(
            INotificationRepository notificationRepository,
            IBackgroundJobService backgroundJobService)
        {
            _notificationRepository = notificationRepository;
            _backgroundJobService = backgroundJobService;
        }

        public async Task CreateAsync(
            Guid recipientId,
            Guid? actorId,
            NotificationType type,
            NotificationEntityType entityType,
            Guid? entityId,
            NotificationChannel channels,
            string title,
            string message,
            CancellationToken cancellationToken = default)
        {
            var notification = Notification.Create(
                recipientId,
                actorId,
                type,
                entityType,
                entityId,
                channels,
                title,
                message);

            await _notificationRepository.AddAsync(
                notification,
                cancellationToken);

            _backgroundJobService.Schedule<INotificationDeliveryJob>(
                job => job.ExecuteAsync(notification.Id),
                TimeSpan.FromSeconds(30));
        }
    }
}