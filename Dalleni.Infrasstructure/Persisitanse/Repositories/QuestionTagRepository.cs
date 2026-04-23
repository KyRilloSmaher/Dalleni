using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class QuestionTagRepository : Repository<QuestionTag>, IQuestionTagRepository
    {
        public QuestionTagRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<QuestionTag>> GetByQuestionIdAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.Tag)
                .Where(x => x.QuestionId == questionId)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<QuestionTag>> GetByTagIdAsync(Guid tagId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.Question)
                .Where(x => x.TagId == tagId)
                .ToListAsync(cancellationToken);

        public Task<bool> ExistsAsync(Guid questionId, Guid tagId, CancellationToken cancellationToken = default)
            => GetQuery().AnyAsync(x => x.QuestionId == questionId && x.TagId == tagId, cancellationToken);

        public async Task<IEnumerable<Guid>> GetTagIdsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
         => await GetQuery()
                .Where(x => x.QuestionId == questionId)
                .Select(x => x.TagId)
                .ToListAsync(cancellationToken);
    }
}
