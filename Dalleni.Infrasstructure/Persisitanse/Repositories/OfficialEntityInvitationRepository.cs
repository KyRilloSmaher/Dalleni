using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class OfficialEntityInvitationRepository
        : Repository<OfficialEntityInvitation>,
          IOfficialEntityInvitationRepository
    {
        public OfficialEntityInvitationRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets an invitation using its hashed token.
        /// Used when accepting an invitation.
        /// </summary>
        public async Task<OfficialEntityInvitation?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(
                    x => x.TokenHash == tokenHash,
                    cancellationToken);
        }

        /// <summary>
        /// Gets an existing pending invitation for an entity
        /// and email address.
        /// </summary>
        public async Task<OfficialEntityInvitation?> GetPendingInvitationAsync(
            Guid officialEntityId,
            string email,
            CancellationToken cancellationToken = default)
        {
            var normalizedEmail = email.Trim().ToUpperInvariant();

            return await DbSet
                .FirstOrDefaultAsync(
                    x =>
                        x.OfficialEntityId == officialEntityId &&
                        x.Email.ToUpper() == normalizedEmail &&
                        !x.IsAccepted &&
                        x.ExpiresAt > DateTime.UtcNow,
                    cancellationToken);
        }
    }
}