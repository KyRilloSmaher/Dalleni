using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IOfficialEntityRepository : IRepository<OfficialEntity>
    {
        Task<IEnumerable<OfficialEntity>> GetByOwnerIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<OfficialEntity>> GetVerifiedAsync(bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<OfficialEntity>> SearchAsync(string keyword, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
