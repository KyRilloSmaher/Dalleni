using Dalleni.Domin.DomainEvents;

namespace Dalleni.Domin.DomainEvents.Events
{
    public class NotificationCreatedDomainEvent : DomainEvent
    {
        public Guid NotificationId { get; }

        public NotificationCreatedDomainEvent(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}