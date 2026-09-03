using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        public RatingRepository(ApplicationDbContext context) : base(context)
        {
        }

       public async Task<double> GetAverageRatingForServiceAsync(Guid serviceId)
        {
            return await DbSet
                .Where(r => r.ServiceId == serviceId && !r.IsDeleted)
                .Select(r => (double?)r.Value)
                .AverageAsync() ?? 0.0;
        }

        public async Task<IEnumerable<Rating>> GetRatingsForServiceAsync(Guid serviceId)
        {
            return await DbSet
                .Where(r => r.ServiceId == serviceId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Rating?> GetUserRatingForServiceAsync(Guid serviceId, Guid userId)
        {
            return await DbSet
                .FirstOrDefaultAsync(r => r.ServiceId == serviceId && r.UserId == userId && !r.IsDeleted);
        }

        public async Task<IEnumerable<Rating>> GetUserRatings(Guid UserId)
        {
            return await DbSet
                .Where(r => r.UserId == UserId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
