using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class ExternalLoginRepository : Repository<ExternalLogin>, IExternalLoginRepository
    {
        public ExternalLoginRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<ExternalLogin?> GetByProviderAsync(string provider, string providerKey, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderKey == providerKey, asTracked, cancellationToken);

        public async Task<IEnumerable<ExternalLogin>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQuery(asTracked)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LastUsedAt)
                .ToListAsync(cancellationToken);

        public Task<bool> ExistsAsync(string provider, string providerKey, CancellationToken cancellationToken = default)
            => GetQuery().AnyAsync(x => x.Provider == provider && x.ProviderKey == providerKey, cancellationToken);
    }
}
