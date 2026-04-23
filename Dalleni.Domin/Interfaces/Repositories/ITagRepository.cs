using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Task<Tag?> GetByNormalizedNameAsync(string normalizedName, bool asTracked = false, CancellationToken cancellationToken = default);
        public Task<Tag?> GetBySlugAsync(string slug, bool asTracked = false, CancellationToken cancellationToken = default);
        public  Task<IEnumerable<Tag>> GetByNormalizedNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
        public  Task<IEnumerable<Tag>> SearchAsync(string keyword, CancellationToken cancellationToken = default);
        public  Task<IQueryable<Tag>> GetTopTagsAsync(CancellationToken cancellationToken = default);
        public Task<bool> ExistsByNormalizedNameAsync(string normalizedName, CancellationToken cancellationToken = default);
    }
}
