using Dalleni.Domin.Enums;
using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<Vote?> GetUserVoteForQuestionAsync(Guid userId, Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<Vote?> GetUserVoteForAnswerAsync(Guid userId, Guid answerId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<int> CountQuestionVotesAsync(Guid questionId, VoteType type, CancellationToken cancellationToken = default);
        Task<int> CountAnswerVotesAsync(Guid answerId, VoteType type, CancellationToken cancellationToken = default);
        Task<bool> HasUserVotedQuestionAsync(Guid userId, Guid questionId, CancellationToken cancellationToken = default);
        Task<bool> HasUserVotedAnswerAsync(Guid userId, Guid answerId, CancellationToken cancellationToken = default);
    }
}
