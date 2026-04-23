using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<RefreshToken?> GetByTokenAsync(string token, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.Token == token, asTracked, cancellationToken);

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQuery(asTracked)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQuery(asTracked)
                .Where(x => x.UserId == userId && !x.IsRevoked && !x.IsExpired)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
    }
}
