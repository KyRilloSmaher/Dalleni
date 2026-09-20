using System.Linq.Expressions;

namespace Dalleni.Application.Services.BackgroundJobs
{
    public interface IBackgroundJobService
    {
        string Enqueue(Expression<Func<Task>> methodCall);

        string Enqueue<T>(Expression<Func<T, Task>> methodCall);

        string Schedule(Expression<Func<Task>> methodCall,TimeSpan delay);

        string Schedule<T>(Expression<Func<T, Task>> methodCall,TimeSpan delay);
    }
}