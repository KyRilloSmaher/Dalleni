using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext context) : base(context)
        {
        }

        // -----------------------------
        // Details
        // -----------------------------
        // For complex includes, use a dedicated method
        protected IQueryable<Question> GetDetailedQuestionQuery(bool asTracked = false)
        {
            return GetQuery(asTracked)
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                .Include(x => x.Answers)
                    .ThenInclude(a => a.User)
                .Include(x => x.Comments);
        }

        public Task<Question?> GetDetailsAsync(Guid id, bool asTracked = false, CancellationToken cancellationToken = default)
            => GetDetailedQuestionQuery(asTracked)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        // -----------------------------
        // Basic Filters
        // -----------------------------
        public async Task<IEnumerable<Question>> GetByCategoryIdAsync(Guid categoryId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetDetailedQuestionQuery(asTracked)
                .Where(x => x.CategoryId == categoryId && !x.IsClosed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetDetailedQuestionQuery(asTracked)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Question>> GetOpenQuestionsAsync(bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetDetailedQuestionQuery(asTracked)
                .Where(x => !x.IsClosed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        // -----------------------------
        //  Search
        // -----------------------------
        public async Task<IQueryable<Question>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
        {
            keyword = keyword.Trim();

            return  GetDetailedQuestionQuery(false)
                .Where(x =>
                    EF.Functions.Like(x.Title, $"%{keyword}%") ||
                    EF.Functions.Like(x.Content, $"%{keyword}%"))
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.CreatedAt);
        }

        // -----------------------------
        //  Top / Popular
        // -----------------------------
        public async Task<IEnumerable<Question>> GetTopQuestionsAsync(int count, CancellationToken cancellationToken = default)
            => await GetDetailedQuestionQuery(false)
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Views)
                .Take(count)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Question>> GetMostViewedAsync(int count, CancellationToken cancellationToken = default)
            => await GetDetailedQuestionQuery(false)
                .OrderByDescending(x => x.Views)
                .Take(count)
                .ToListAsync(cancellationToken);

        // -----------------------------
        //  Unanswered
        // -----------------------------
        public async Task<IEnumerable<Question>> GetUnansweredQuestionsAsync(CancellationToken cancellationToken = default)
            => await GetDetailedQuestionQuery(false)
                .Where(x => x.AnswersCount == 0 && !x.IsClosed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        // -----------------------------
        // Tag-Based Queries
        // -----------------------------
        public async Task<IQueryable<Question>> GetByTagIdAsync(Guid tagId, CancellationToken cancellationToken = default)
            =>  GetDetailedQuestionQuery(false)
                .Where(q => q.QuestionTags.Any(qt => qt.TagId == tagId))
                .OrderByDescending(x => x.CreatedAt);

        public async Task<IEnumerable<Question>> GetByTagIdsAsync(IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default)
        {
            var tagIdsList = tagIds.ToList();

            return await GetDetailedQuestionQuery(false)
                .Where(q => q.QuestionTags.Any(qt => tagIdsList.Contains(qt.TagId)))
                .OrderByDescending(q => q.Score)
                .ToListAsync(cancellationToken);
        }

        // -----------------------------
        //Related Questions
        // -----------------------------
        public async Task<IEnumerable<Question>> GetRelatedQuestionsAsync(Guid questionId, int count, CancellationToken cancellationToken = default)
        {
            var tagIds = await Context.Set<QuestionTag>()
                .Where(qt => qt.QuestionId == questionId)
                .Select(qt => qt.TagId)
                .ToListAsync(cancellationToken);

            return await GetDetailedQuestionQuery(false)
                .Where(q => q.Id != questionId &&
                            q.QuestionTags.Any(qt => tagIds.Contains(qt.TagId)))
                .OrderByDescending(q => q.Score)
                .ThenByDescending(q => q.Views)
                .Take(count)
                .ToListAsync(cancellationToken);
        }

        // -----------------------------
        // Questions (Ranking)
        // -----------------------------
        public async Task<IQueryable<Question>> GetHotQuestionsAsync( CancellationToken cancellationToken = default)
        {

            return  GetDetailedQuestionQuery(false)
                .OrderByDescending(q => q.Score);
        }
    }
}