using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    /// <summary>
    /// Defines data access operations specific to user devices.
    /// </summary>
    /// <remarks>
    /// Extends the generic repository with operations required to manage
    /// push notification device registrations.
    /// </remarks>
    public interface IUserDeviceRepository : IRepository<UserDevice>
    {
        /// <summary>
        /// Retrieves all devices registered for a specific user.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user.
        /// </param>
        /// <param name="activeOnly">
        /// Indicates whether only active devices should be returned.
        /// Defaults to true.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task containing the user's registered devices.
        /// </returns>
        Task<IEnumerable<UserDevice>> GetByUserIdAsync(
            Guid userId,
            bool activeOnly = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a device by its push notification token.
        /// </summary>
        /// <param name="deviceToken">
        /// The push notification token to search for.
        /// </param>
        /// <param name="asTracked">
        /// Indicates whether the returned device should be tracked.
        /// Defaults to true because the device may need to be updated.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// The matching device if found; otherwise, null.
        /// </returns>
        Task<UserDevice?> GetByDeviceTokenAsync(
            string deviceToken,
            bool asTracked = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all active devices registered for a specific user.
        /// </summary>
        /// <param name="userId">
        /// The unique identifier of the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// A task containing the user's active devices.
        /// </returns>
        Task<IEnumerable<UserDevice>> GetActiveByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}