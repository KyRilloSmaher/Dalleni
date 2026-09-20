using Dalleni.Application.DTOs.Notifications;
using Dalleni.Application.Services.Notifications;
using Dalleni.Domin.Interfaces.Repositories;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;

namespace Dalleni.Infrastructure.Services.Notifications
{
    public class FirebaseNotificationPushService : INotificationPushService
    {
        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FirebaseNotificationPushService> _logger;

        public FirebaseNotificationPushService(
            IUserDeviceRepository userDeviceRepository,
            IUnitOfWork unitOfWork,
            ILogger<FirebaseNotificationPushService> logger)
        {
            _userDeviceRepository = userDeviceRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task SendAsync(
            Guid recipientId,
            NotificationPushPayloadDto payload,
            CancellationToken cancellationToken = default)
        {
            var devices =
                await _userDeviceRepository.GetByUserIdAsync(
                    recipientId,
                    activeOnly: true,
                    cancellationToken);

            var activeDevices = devices
                .Where(device => !string.IsNullOrWhiteSpace(device.DeviceToken))
                .ToList();

            if (activeDevices.Count == 0)
            {
                _logger.LogDebug(
                    "No active devices found for user {UserId}.",
                    recipientId);

                return;
            }

            var hasChanges = false;

            foreach (var device in activeDevices)
            {
                try
                {
                    var message = new Message
                    {
                        Token = device.DeviceToken,

                        Notification = new Notification
                        {
                            Title = payload.Title,
                            Body = payload.Body
                        },

                        Data = new Dictionary<string, string>
                        {
                            ["notificationId"] =
                                payload.NotificationId.ToString(),

                            ["type"] =
                                payload.Type.ToString(),

                            ["entityType"] =
                                payload.EntityType.ToString(),

                            ["entityId"] =
                                payload.EntityId?.ToString()
                                ?? string.Empty,

                            ["actorId"] =
                                payload.ActorId?.ToString()
                                ?? string.Empty
                        }
                    };

                    var messageId =
                        await FirebaseMessaging.DefaultInstance.SendAsync(
                            message,
                            cancellationToken);

                    device.UpdateLastUsed();

                    hasChanges = true;

                    _logger.LogDebug(
                        "FCM notification {NotificationId} sent to device {DeviceId}. MessageId: {MessageId}",
                        payload.NotificationId,
                        device.Id,
                        messageId);
                }
                catch (FirebaseMessagingException ex)
                {
                    if (IsInvalidToken(ex))
                    {
                        device.MarkAsInactive();

                        hasChanges = true;

                        _logger.LogInformation(
                            "Deactivated invalid FCM device token for device {DeviceId}.",
                            device.Id);

                        continue;
                    }

                    _logger.LogWarning(
                        ex,
                        "Failed to send FCM notification {NotificationId} to device {DeviceId}.",
                        payload.NotificationId,
                        device.Id);
                }
            }

            if (hasChanges)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        private static bool IsInvalidToken(
            FirebaseMessagingException exception)
        {
            return exception.MessagingErrorCode
                is MessagingErrorCode.Unregistered
                or MessagingErrorCode.InvalidArgument;
        }
    }
}