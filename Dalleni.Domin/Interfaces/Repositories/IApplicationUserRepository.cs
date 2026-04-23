using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser?> GetByEmailAsync(string email, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<ApplicationUser?> GetByUserNameAsync(string userName, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<ApplicationUser>> SearchAsync(string keyword, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<IEnumerable<ApplicationUser>> GetTopUsersByReputationAsync(int count, CancellationToken cancellationToken = default);

        Task<IEnumerable<ApplicationUser>> GetTopContributorsAsync(int count, CancellationToken cancellationToken = default);
        // Based on AnswersCount or AcceptedAnswersCount
        Task<ApplicationUser?> GetUserWithStatsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
