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

        public override async Task<Answer?> GetByIdAsync(Guid id, bool asTracked = true, CancellationToken cancellationToken = default)
        {
            var query = GetQueryWithIncludes(asTracked, x => x.User, x => x.Question);
            return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default)
        {
            var answersQuery = await GetAllAsQueryableAsync(false, cancellationToken);
            
            var answers = await answersQuery
                .Where(a => a.QuestionId == questionId)
                .Include(a => a.User)
                .OrderByDescending(a => a.IsAccepted)
                .ThenByDescending(a => a.Score)
                .ToListAsync(cancellationToken);
            
            return answers;
        }

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
