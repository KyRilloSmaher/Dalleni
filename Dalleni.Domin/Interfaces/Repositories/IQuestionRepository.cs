using Dalleni.Domin.Models;

namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<Question?> GetDetailsAsync(Guid id, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetByCategoryIdAsync(Guid categoryId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetOpenQuestionsAsync(bool asTracked = false, CancellationToken cancellationToken = default);
        Task<IQueryable<Question>> SearchAsync(string keyword, CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetTopQuestionsAsync(int count, CancellationToken cancellationToken = default);
        Task<IQueryable<Question>> GetByTagIdAsync(Guid tagId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetByTagIdsAsync(IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default);
        Task<IQueryable<Question>> GetHotQuestionsAsync( CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetUnansweredQuestionsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetMostViewedAsync(int count, CancellationToken cancellationToken = default);
        Task<IEnumerable<Question>> GetRelatedQuestionsAsync(Guid questionId, int count, CancellationToken cancellationToken = default);

    }
}
