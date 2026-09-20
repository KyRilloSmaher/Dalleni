using Dalleni.Domin.Models;
using Dalleni.Domin.Enums;

namespace Dalleni.Domin.Interfaces.Repositories
{
    /// <summary>
    /// Defines data access operations specific to notifications.
    /// </summary>
    /// <remarks>
    /// Extends the generic repository with notification-specific queries
    /// that are commonly required by the notification subsystem.
    /// </remarks>
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// Retrieves all notifications belonging to a specific recipient.
        /// </summary>
        /// <param name="recipientId">
        /// The unique identifier of the user receiving the notifications.
        /// </param>
        /// <param name="asTracked">
        /// Indicates whether the returned notifications should be tracked.
        /// Defaults to false because notification history is normally read-only.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task containing the recipient's notifications.
        /// </returns>
        Task<IQueryable<Notification>> GetByRecipientIdAsync(
            Guid recipientId,
            bool asTracked = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves unread notifications belonging to a specific recipient.
        /// </summary>
        /// <param name="recipientId">
        /// The unique identifier of the user receiving the notifications.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task containing the recipient's unread notifications.
        /// </returns>
        Task<IQueryable<Notification>> GetUnreadByRecipientIdAsync(
            Guid recipientId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the number of unread notifications for a specific recipient.
        /// </summary>
        /// <param name="recipientId">
        /// The unique identifier of the user receiving the notifications.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task containing the number of unread notifications.
        /// </returns>
        Task<int> GetUnreadCountAsync(
            Guid recipientId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves notifications associated with a specific domain entity.
        /// </summary>
        /// <param name="entityType">
        /// The type of entity associated with the notifications.
        /// </param>
        /// <param name="entityId">
        /// The identifier of the associated entity.
        /// </param>
        /// <param name="asTracked">
        /// Indicates whether the returned notifications should be tracked.
        /// Defaults to false.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task containing notifications associated with the entity.
        /// </returns>
        Task<IEnumerable<Notification>> GetByEntityAsync(
            NotificationEntityType entityType,
            Guid entityId,
            bool asTracked = false,
            CancellationToken cancellationToken = default);
    }
}