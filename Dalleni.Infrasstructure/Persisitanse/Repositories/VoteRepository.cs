using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class VoteRepository : Repository<Vote>, IVoteRepository
    {
        public VoteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Vote?> GetUserVoteForQuestionAsync(Guid userId, Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.UserId == userId && x.QuestionId == questionId, asTracked, cancellationToken);

        public Task<Vote?> GetUserVoteForAnswerAsync(Guid userId, Guid answerId, bool asTracked = false, CancellationToken cancellationToken = default)
            => FirstOrDefaultAsync(x => x.UserId == userId && x.AnswerId == answerId, asTracked, cancellationToken);

        public Task<int> CountQuestionVotesAsync(Guid questionId, VoteType type, CancellationToken cancellationToken = default)
            => GetQuery().CountAsync(x => x.QuestionId == questionId && x.Type == type, cancellationToken);

        public Task<int> CountAnswerVotesAsync(Guid answerId, VoteType type, CancellationToken cancellationToken = default)
            => GetQuery().CountAsync(x => x.AnswerId == answerId && x.Type == type, cancellationToken);

        public async Task<bool> HasUserVotedQuestionAsync(Guid userId, Guid questionId, CancellationToken cancellationToken = default)
          => await GetQuery().AnyAsync(x => x.UserId == userId && x.QuestionId == questionId, cancellationToken);

        public async Task<bool> HasUserVotedAnswerAsync(Guid userId, Guid answerId, CancellationToken cancellationToken = default)
         => await GetQuery().AnyAsync(x => x.UserId == userId && x.AnswerId == answerId, cancellationToken);
    }
}
