using Dalleni.Domin.Enums;

namespace Dalleni.Application.DTOs.Notifications
{
    public class NotificationPushPayloadDto
    {
        public Guid NotificationId { get; set; }

        public NotificationType Type { get; set; }

        public NotificationEntityType EntityType { get; set; }

        public Guid? EntityId { get; set; }

        public Guid? ActorId { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;
    }
}