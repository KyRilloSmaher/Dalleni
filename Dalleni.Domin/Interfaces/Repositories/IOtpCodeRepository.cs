using Dalleni.Domin.Enums;
using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IOtpCodeRepository : IRepository<OtpCode>
    {
        Task<OtpCode?> GetByCodeAsync(Guid userId, string code, OtpType purpose, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<OtpCode?> GetLatestActiveAsync(Guid userId, OtpType purpose, bool asTracked = false, CancellationToken cancellationToken = default);
    }
}
