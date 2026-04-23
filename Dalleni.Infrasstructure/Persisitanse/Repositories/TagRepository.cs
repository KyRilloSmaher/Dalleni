using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }


        // -----------------------------
        // 🔥 Core Lookups (Normalized)
        // -----------------------------
        public async Task<Tag?> GetByNormalizedNameAsync(string normalizedName, bool asTracked = false, CancellationToken cancellationToken = default)
            => await FirstOrDefaultAsync(
                x => x.NormalizedName == normalizedName.Trim().ToLower(),
                asTracked,
                cancellationToken);

        public async Task<Tag?> GetBySlugAsync(string slug, bool asTracked = false, CancellationToken cancellationToken = default)
            => await FirstOrDefaultAsync(
                x => x.Slug == slug.Trim().ToLower(),
                asTracked,
                cancellationToken);

        // -----------------------------
        // 🔥 Bulk Fetch (Important for tagging flow)
        // -----------------------------
        public async Task<IEnumerable<Tag>> GetByNormalizedNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default)
        {
            var normalized = names
                .Select(x => x.Trim().ToLower())
                .Distinct()
                .ToList();

            return await GetQuery()
                .Where(x => normalized.Contains(x.NormalizedName))
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }

        // -----------------------------
        // 🔥 Search (Auto-suggest)
        // -----------------------------
        public async Task<IEnumerable<Tag>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
        {
            keyword = keyword.Trim().ToLower();

            return await GetQuery()
                .Where(x =>
                    EF.Functions.Like(x.Name, $"%{keyword}%") ||
                    EF.Functions.Like(x.NormalizedName, $"%{keyword}%"))
                .OrderByDescending(x => x.UsageCount)
                .ThenBy(x => x.Name)
                .Take(20) // limit for auto-complete
                .ToListAsync(cancellationToken);
        }

        // -----------------------------
        // 🔥 Popular Tags
        // -----------------------------
        public async Task<IQueryable<Tag>> GetTopTagsAsync(CancellationToken cancellationToken = default)
            =>  GetQuery()
                .OrderByDescending(x => x.UsageCount);

        // -----------------------------
        // 🔥 Existence Checks
        // -----------------------------
        public async Task<bool> ExistsByNormalizedNameAsync(string normalizedName, CancellationToken cancellationToken = default)
            =>await  GetQuery()
                .AnyAsync(x => x.NormalizedName == normalizedName.Trim().ToLower(), cancellationToken);
    }
}
