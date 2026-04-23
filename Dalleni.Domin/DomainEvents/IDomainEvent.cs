using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.DomainEvents
{
    public interface IDomainEvent
    {
        public DateTime OccurredOn { get; }
    }
}
