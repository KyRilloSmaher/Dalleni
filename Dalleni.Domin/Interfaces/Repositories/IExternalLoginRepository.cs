using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IExternalLoginRepository : IRepository<ExternalLogin>
    {
        Task<ExternalLogin?> GetByProviderAsync(string provider, string providerKey, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<ExternalLogin>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string provider, string providerKey, CancellationToken cancellationToken = default);
    }
}
