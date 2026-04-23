using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class OfficialEntityRepository : Repository<OfficialEntity>, IOfficialEntityRepository
    {
        public OfficialEntityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OfficialEntity>> GetByOwnerIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.Services)
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<OfficialEntity>> GetVerifiedAsync(bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQuery(asTracked)
                .Where(x => x.IsVerified)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<OfficialEntity>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(false, x => x.Services)
                .Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword))
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
            => GetQuery().AnyAsync(x => x.Name == name, cancellationToken);
    }
}
