using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default);
    }
}
