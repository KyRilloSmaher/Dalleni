using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        // -----------------------------
        // Basic Queries
        // -----------------------------
        public Task<ApplicationUser?> GetByEmailAsync(string email, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.Email == email, asTracked, cancellationToken);

        public Task<ApplicationUser?> GetByUserNameAsync(string userName, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.UserName == userName, asTracked, cancellationToken);

        public async Task<IEnumerable<ApplicationUser>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
        {
            keyword = keyword.Trim().ToLower();

            return await GetQuery()
                .Where(x =>
                    x.FullName.ToLower().Contains(keyword) ||
                    (x.Email != null && x.Email.ToLower().Contains(keyword)) ||
                    (x.UserName != null && x.UserName.ToLower().Contains(keyword)))
                .OrderBy(x => x.FullName)
                .ToListAsync(cancellationToken);
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
            => GetQuery().AnyAsync(x => x.Email == email, cancellationToken);

        public Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default)
            => GetQuery().AnyAsync(x => x.UserName == userName, cancellationToken);

        // -----------------------------
        // Stats / Ranking
        // -----------------------------

        public async Task<IEnumerable<ApplicationUser>> GetTopUsersByReputationAsync(int count, CancellationToken cancellationToken = default)
        {
            return await GetQuery()
                .OrderByDescending(x => x.Reputation)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ApplicationUser>> GetTopContributorsAsync(int count, CancellationToken cancellationToken = default)
        {
            return await GetQuery()
                .OrderByDescending(x => x.AcceptedAnswersCount)
                .ThenByDescending(x => x.AnswersCount)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        // -----------------------------
        // Optimized Profile Load
        // -----------------------------
        public async Task<ApplicationUser?> GetUserWithStatsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await GetQuery()
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}