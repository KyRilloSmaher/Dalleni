using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.DomainEvents
{
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
