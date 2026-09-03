using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class OfficialEntityMembershipRepository
        : Repository<OfficialEntityMembership>,
          IOfficialEntityMembershipRepository
    {
        public OfficialEntityMembershipRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Gets a user's membership in a specific official entity.
        /// </summary>
        public async Task<OfficialEntityMembership?> GetByUserAndEntityAsync(
            Guid userId,
            Guid officialEntityId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x =>
                        x.UserId == userId &&
                        x.OfficialEntityId == officialEntityId,
                    cancellationToken);
        }

        /// <summary>
        /// Gets the user's membership.
        /// </summary>
        public async Task<OfficialEntityMembership?> GetByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.UserId == userId,
                    cancellationToken);
        }

        /// <summary>
        /// Gets all official entities where the user
        /// has an active membership.
        /// </summary>
        public async Task<IEnumerable<OfficialEntity>> GetEntitiesForUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x =>
                    x.UserId == userId &&
                    x.IsActive &&
                    !x.OfficialEntity.IsDeleted)
                .Select(x => x.OfficialEntity)
                .ToListAsync(cancellationToken);
        }
    }
}