using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Answer>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<Answer?> GetAcceptedAnswerAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default);
    }
}
