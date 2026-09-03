using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IOfficialEntityMembershipRepository  : IRepository<OfficialEntityMembership>
    {
        Task<OfficialEntityMembership?> GetByUserAndEntityAsync(
            Guid userId,
            Guid officialEntityId,
            CancellationToken cancellationToken = default);
        Task<OfficialEntityMembership?> GetByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
Task<IEnumerable<OfficialEntity>> GetEntitiesForUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

    }
}