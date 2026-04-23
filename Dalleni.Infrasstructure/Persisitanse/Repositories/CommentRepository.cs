using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> GetByQuestionIdAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.User)
                .Where(x => x.QuestionId == questionId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Comment>> GetByAnswerIdAsync(Guid answerId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.User)
                .Where(x => x.AnswerId == answerId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQuery(asTracked)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
    }
}
