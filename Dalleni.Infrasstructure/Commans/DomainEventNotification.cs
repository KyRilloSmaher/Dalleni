using Dalleni.Domin.DomainEvents;
using MediatR;

namespace Dalleni.Infrastructure.Commans
{

    public class DomainEventNotification<T> : INotification where T : IDomainEvent
    {
        public T DomainEvent { get; }

        public DomainEventNotification(T domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}