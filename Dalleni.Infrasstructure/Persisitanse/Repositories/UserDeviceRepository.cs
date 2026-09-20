using Dalleni.Domin.Models;
using Dalleni.Domin.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    /// <summary>
    /// Provides device-specific data access operations for users.
    /// </summary>
    public class UserDeviceRepository: Repository<UserDevice>, IUserDeviceRepository
    {
        public UserDeviceRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserDevice>> GetByUserIdAsync(
            Guid userId,
            bool activeOnly = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<UserDevice> query = GetQuery(false)
                .Where(device => device.UserId == userId);

            if (activeOnly)
            {
                query = query.Where(device => device.IsActive);
            }

            return await query
                .OrderByDescending(device => device.LastUsedAt)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<UserDevice?> GetByDeviceTokenAsync(
            string deviceToken,
            bool asTracked = true,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(deviceToken);

            return await FirstOrDefaultAsync(
                device => device.DeviceToken == deviceToken,
                asTracked,
                cancellationToken);
        }

        /// <inheritdoc />

        public async Task<IEnumerable<UserDevice>> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await GetQuery(false)
                .Where(device => device.UserId == userId && device.IsActive)
                .OrderByDescending(device => device.LastUsedAt)
                .ToListAsync(cancellationToken);
        }
    }
}