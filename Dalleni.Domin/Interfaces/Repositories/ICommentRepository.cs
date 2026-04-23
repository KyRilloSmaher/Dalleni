using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByQuestionIdAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Comment>> GetByAnswerIdAsync(Guid answerId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default);
    }
}
