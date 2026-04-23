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

        public async Task<IEnumerable<Service>> GetByOfficialEntityIdAsync(Guid officialEntityId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQuery(asTracked)
                .Where(x => x.OfficialEntityId == officialEntityId)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Service>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(false, x => x.OfficialEntity)
                .Where(x => x.Name.Contains(keyword) || x.Description.Contains(keyword) || x.RequiredDocuments.Contains(keyword))
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
    }
}
