using Dalleni.Application.Services.BackgroundJobs;
using Hangfire;
using System.Linq.Expressions;

namespace Dalleni.Infrastructure.Services.BackgroundJobs
{
    public class HangfireBackgroundJobService: IBackgroundJobService
    {
        public string Enqueue(Expression<Func<Task>> methodCall)
        {
            ArgumentNullException.ThrowIfNull(methodCall);
            return BackgroundJob.Enqueue(methodCall);
        }
        public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
        {  
            ArgumentNullException.ThrowIfNull(methodCall);
            return BackgroundJob.Enqueue(methodCall);
        }
        public string Schedule(Expression<Func<Task>> methodCall,TimeSpan delay)
        {
            ArgumentNullException.ThrowIfNull(methodCall);
            return BackgroundJob.Schedule(methodCall,delay);
        }
        public string Schedule<T>(Expression<Func<T, Task>> methodCall,TimeSpan delay)
        {
            ArgumentNullException.ThrowIfNull(methodCall);
            return BackgroundJob.Schedule(methodCall,delay);
        }
    }
}