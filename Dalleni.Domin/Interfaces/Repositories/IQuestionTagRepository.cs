using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IQuestionTagRepository : IRepository<QuestionTag>
    {
        Task<IEnumerable<QuestionTag>> GetByQuestionIdAsync(Guid questionId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<QuestionTag>> GetByTagIdAsync(Guid tagId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid questionId, Guid tagId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Guid>> GetTagIdsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
    }
}
