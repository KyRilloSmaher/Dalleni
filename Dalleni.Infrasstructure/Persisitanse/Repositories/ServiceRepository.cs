using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<IQueryable<Service>> GetAllAsQueryableAsync(bool asTracked = false, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
                        
            return GetQueryWithIncludes(asTracked, x => x.OfficialEntity, x=> x.Category)
                    .OrderBy(x => x.Name).AsQueryable();
                  
        }

        public override async Task<Service?> GetByIdAsync(Guid id, bool asTracked = false, CancellationToken cancellationToken = default)
        {
            return await GetQueryWithIncludes(asTracked, x => x.OfficialEntity, x=> x.Category)
                        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Service>> GetByOfficialEntityIdAsync(Guid officialEntityId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(false, x => x.OfficialEntity, x=> x.Category)
                .Where(x => x.OfficialEntityId == officialEntityId)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Service>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(false, x => x.OfficialEntity, x=> x.Category)
                .Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.RequiredDocuments.Contains(keyword))
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        public async Task<IEnumerable<Service>> getByCategoryAsync(Guid CategoryId,CancellationToken cancellationToken = default)
        =>  await GetQueryWithIncludes(false, x => x.OfficialEntity, x=> x.Category)
                .Where(x => x.CategoryId== CategoryId)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
    }
}
