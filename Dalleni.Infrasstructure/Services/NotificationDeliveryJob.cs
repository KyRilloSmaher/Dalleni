using AutoMapper;
using Dalleni.Application.DTOs.Notifications;
using Dalleni.Application.DTOs.Responses.Notifications;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Services.Notifications
{
    public class NotificationDeliveryJob : INotificationDeliveryJob
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationRealtimeService _realtimeService;
        private readonly INotificationPushService _pushService;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationDeliveryJob> _logger;

        public NotificationDeliveryJob(
            INotificationRepository notificationRepository,
            INotificationRealtimeService realtimeService,
            INotificationPushService pushService,
            IMapper mapper,
            ILogger<NotificationDeliveryJob> logger)
        {
            _notificationRepository = notificationRepository;
            _realtimeService = realtimeService;
            _pushService = pushService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid notificationId)
        {
            var notification =
                await _notificationRepository.GetByIdAsync(
                    notificationId,
                    asTracked: false,
                    CancellationToken.None);

            if (notification is null)
            {
                _logger.LogWarning(
                    "Notification {NotificationId} was not found.",
                    notificationId);

                return;
            }

            var notificationDto =
                _mapper.Map<NotificationResponseDto>(
                    notification);

            if (notification.Channels.HasFlag(NotificationChannel.RealTime))
            {
                await SendRealTimeNotificationAsync(
                    notification.RecipientId,
                    notificationDto);
            }

            if (notification.Channels.HasFlag(NotificationChannel.Push))
            {
                var pushPayload =
                    BuildPushPayload(notification);

                await SendPushNotificationAsync(
                    notification.RecipientId,
                    pushPayload);
            }
        }

        private async Task SendRealTimeNotificationAsync(Guid recipientId,NotificationResponseDto notification)
        {
            try
            {
                await _realtimeService.SendAsync(
                    recipientId,
                    notification);

                _logger.LogDebug(
                    "Real-time notification {NotificationId} sent to user {UserId}.",
                    notification.Id,
                    recipientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send real-time notification {NotificationId} to user {UserId}.",
                    notification.Id,
                    recipientId);

                throw;
            }
        }

        private async Task SendPushNotificationAsync(Guid recipientId,NotificationPushPayloadDto payload)
        {
            try
            {
                await _pushService.SendAsync(
                    recipientId,
                    payload);

                _logger.LogDebug(
                    "Push notification {NotificationId} sent to user {UserId}.",
                    payload.NotificationId,
                    recipientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send push notification {NotificationId} to user {UserId}.",
                    payload.NotificationId,
                    recipientId);

                throw;
            }
        }

        private static NotificationPushPayloadDto BuildPushPayload(Notification notification)
        {
            return new NotificationPushPayloadDto
            {
                NotificationId = notification.Id,
                Type = notification.Type,
                EntityType = notification.EntityType,
                EntityId = notification.EntityId,
                ActorId = notification.ActorId,

                // These now come from the Notification entity.
                Title = notification.Title,
                Body = notification.Message
            };
        }
    }
}