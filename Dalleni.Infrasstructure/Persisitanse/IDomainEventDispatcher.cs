using Dalleni.Infrastructure.Persisitanse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Commans
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(ApplicationDbContext context);
    }
}
