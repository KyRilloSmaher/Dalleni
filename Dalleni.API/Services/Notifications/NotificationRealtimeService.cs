using Dalleni.API.Hubs;
using Dalleni.Application.DTOs.Responses.Notifications;
using Dalleni.Application.Services.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Dalleni.API.Services.Notifications
{
    public class NotificationRealtimeService : INotificationRealtimeService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationRealtimeService(
            IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(
            Guid recipientId,
            NotificationResponseDto notification,
            CancellationToken cancellationToken = default)
        {
            var groupName = $"user:{recipientId}";

            await _hubContext
                .Clients
                .Group(groupName)
                .SendAsync(
                    "NotificationReceived",
                    notification,
                    cancellationToken);
        }
    }
}