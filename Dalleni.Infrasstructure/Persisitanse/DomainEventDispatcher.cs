using Dalleni.Application.Commans;
using Dalleni.Application.Common;
using Dalleni.Domin.DomainEvents;
using Dalleni.Domin.Models.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(ApplicationDbContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<DomainEntity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                var notification = Wrap(domainEvent);
                await _mediator.Publish(notification);
            }

            foreach (var entity in domainEntities)
            {
                entity.ClearDomainEvents();
            }
        }

        private static INotification Wrap(IDomainEvent domainEvent)
        {
            var type = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            return (INotification)Activator.CreateInstance(type, domainEvent)!;
        }
    }
}