using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class OtpCodeRepository : Repository<OtpCode>, IOtpCodeRepository
    {
        public OtpCodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<OtpCode?> GetByCodeAsync(Guid userId, string code, OtpType purpose, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code && x.Purpose == purpose, asTracked, cancellationToken);

        public Task<OtpCode?> GetLatestActiveAsync(Guid userId, OtpType purpose, bool asTracked = false, CancellationToken cancellationToken = default)
            => GetQuery(asTracked)
                .Where(x => x.UserId == userId && x.Purpose == purpose && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(x => x.ExpiresAt)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
