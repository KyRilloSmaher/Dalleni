using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<double> GetAverageRatingForServiceAsync(Guid serviceId);
        Task<Rating?> GetUserRatingForServiceAsync(Guid serviceId, Guid userId);
        Task<IEnumerable<Rating>> GetRatingsForServiceAsync(Guid serviceId);
        Task<IEnumerable<Rating>> GetUserRatings(Guid UserId);
    }
}