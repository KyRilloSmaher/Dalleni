using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<IEnumerable<Service>> GetByOfficialEntityIdAsync(Guid officialEntityId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Service>> SearchAsync(string keyword, CancellationToken cancellationToken = default);
    }
}
