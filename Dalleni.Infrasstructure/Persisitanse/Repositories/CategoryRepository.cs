using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Category?> GetByNameAsync(string name, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.Name.Contains(name), asTracked, cancellationToken);

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
            => GetQuery().AnyAsync(x => x.Name.Contains(name), cancellationToken);
                    public async Task<Dictionary<Guid, ApplicationUser>> GetUsersByIdsAsync( List<Guid> userIds, CancellationToken cancellationToken = default)
        {
            if (!userIds.Any())
                return new Dictionary<Guid, ApplicationUser>();

            return await Context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u, cancellationToken);
        }
    }
}
