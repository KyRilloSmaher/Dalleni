using Dalleni.Domin.Models;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    /// <summary>
    /// Provides notification-specific data access operations.
    /// </summary>
    public class NotificationRepository: Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<IQueryable<Notification>> GetByRecipientIdAsync(
            Guid recipientId,
            bool asTracked = false,
            CancellationToken cancellationToken = default)
        {
            return  GetQuery(asTracked)
                                .Where(notification => notification.RecipientId == recipientId)
                                .OrderByDescending(notification => notification.CreatedAt);
        }

        /// <inheritdoc />
        public async Task<IQueryable<Notification>> GetUnreadByRecipientIdAsync(
            Guid recipientId,
            CancellationToken cancellationToken = default)
        {
            return  GetQuery(false)
                                .Where(notification => notification.RecipientId == recipientId && !notification.IsRead)
                                 .OrderByDescending(notification => notification.CreatedAt);
        }

        /// <inheritdoc />
        public async Task<int> GetUnreadCountAsync(
            Guid recipientId,
            CancellationToken cancellationToken = default)
        {
            return await GetQuery(false)
                .CountAsync(
                    notification =>
                        notification.RecipientId == recipientId &&
                        !notification.IsRead,
                    cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Notification>> GetByEntityAsync(
            NotificationEntityType entityType,
            Guid entityId,
            bool asTracked = false,
            CancellationToken cancellationToken = default)
        {
            return await FindListAsync(
                notification =>
                    notification.EntityType == entityType &&
                    notification.EntityId == entityId,
                asTracked,
                cancellationToken);
        }
    }
}