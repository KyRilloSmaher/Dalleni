using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IOfficialEntityInvitationRepository : IRepository<OfficialEntityInvitation>
    {
        Task<OfficialEntityInvitation?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken cancellationToken = default);

        Task<OfficialEntityInvitation?> GetPendingInvitationAsync(
            Guid officialEntityId,
            string email,
            CancellationToken cancellationToken = default);

    }
}