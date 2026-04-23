using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.User)
                .Where(x => x.QuestionId == questionId)
                .OrderByDescending(x => x.IsAccepted)
                .ThenByDescending(x => x.Score)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Answer>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.Question)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public Task<Answer?> GetAcceptedAnswerAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default)
            => GetQueryWithIncludes(asTracked, x => x.User)
                .FirstOrDefaultAsync(x => x.QuestionId == questionId && x.IsAccepted, cancellationToken);
    }
}
