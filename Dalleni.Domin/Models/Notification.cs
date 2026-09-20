using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    public class Notification: DomainEntity
    {
        private Notification()
        {
        }

        private Notification(
            Guid recipientId,
            Guid? actorId,
            NotificationType type,
            NotificationEntityType entityType,
            Guid? entityId,
            NotificationChannel channels,
            string title,
            string message)
        {
            Id = Guid.NewGuid();

            RecipientId = recipientId;
            ActorId = actorId;

            Type = type;
            EntityType = entityType;
            EntityId = entityId;

            Channels = channels;

            Title = title;
            Message = message;

            IsRead = false;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        /// <summary>
        /// The user who should receive the notification.
        /// </summary>
        public Guid RecipientId { get; private set; }

        /// <summary>
        /// The user who caused the notification.
        /// For example, the user who upvoted an answer.
        /// Null for system-generated notifications.
        /// </summary>
        public Guid? ActorId { get; private set; }

        /// <summary>
        /// Describes why the notification was created.
        /// </summary>
        public NotificationType Type { get; private set; }

        /// <summary>
        /// Describes the type of entity the notification refers to.
        /// </summary>
        public NotificationEntityType EntityType { get; private set; }

        /// <summary>
        /// The identifier of the entity the notification refers to.
        /// </summary>
        public Guid? EntityId { get; private set; }

        /// <summary>
        /// Specifies the channels through which the notification
        /// should be delivered.
        /// </summary>
        public NotificationChannel Channels { get; private set; }

        public string Title { get; private set; } = null!;

        public string Message { get; private set; } = null!;

        public bool IsRead { get; private set; }


        public DateTime? ReadAt { get; private set; }

        public static Notification Create(
            Guid recipientId,
            Guid? actorId,
            NotificationType type,
            NotificationEntityType entityType,
            Guid? entityId,
            NotificationChannel channels,
            string title,
            string message)
        {
            Validate(
                recipientId,
                actorId,
                channels,
                title,
                message,
                entityId);

           var notification = new Notification(
                recipientId,
                actorId,
                type,
                entityType,
                entityId,
                channels,
                title,
                message);
            return notification;
        }

        public void MarkAsRead()
        {
            if (IsRead)
            {
                return;
            }

            IsRead = true;
            ReadAt = DateTime.UtcNow;
        }

        public void MarkAsUnread()
        {
            if (!IsRead)
            {
                return;
            }

            IsRead = false;
            ReadAt = null;
        }

        public void UpdateChannels(NotificationChannel channels)
        {
            if (channels == NotificationChannel.None)
            {
                throw new ArgumentException(
                    "At least one notification channel must be specified.",
                    nameof(channels));
            }

            Channels = channels;
        }

        private static void Validate(
            Guid recipientId,
            Guid? actorId,
            NotificationChannel channels,
            string title,
            string message,
            Guid? entityId)
        {
            if (recipientId == Guid.Empty)
            {
                throw new ArgumentException(
                    "Notification recipient cannot be empty.",
                    nameof(recipientId));
            }

            if (actorId.HasValue && actorId.Value == Guid.Empty)
            {
                throw new ArgumentException(
                    "Notification actor cannot be empty.",
                    nameof(actorId));
            }

            if (entityId.HasValue && entityId.Value == Guid.Empty)
            {
                throw new ArgumentException(
                    "Notification entity identifier cannot be empty.",
                    nameof(entityId));
            }

            if (channels == NotificationChannel.None)
            {
                throw new ArgumentException(
                    "At least one notification channel must be specified.",
                    nameof(channels));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "Notification title is required.",
                    nameof(title));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(
                    "Notification message is required.",
                    nameof(message));
            }
        }
    }
}