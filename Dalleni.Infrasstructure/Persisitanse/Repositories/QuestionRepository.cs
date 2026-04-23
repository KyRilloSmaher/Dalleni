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
        public Task<Question?> GetDetailsAsync(Guid id, bool asTracked = false, CancellationToken cancellationToken = default)
            => GetQueryWithIncludes(asTracked,
                    x => x.User,
                    x => x.Category,
                    x => x.Answers,
                    x => x.Comments,
                    x => x.QuestionTags)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        // -----------------------------
        // Basic Filters
        // -----------------------------
        public async Task<IEnumerable<Question>> GetByCategoryIdAsync(Guid categoryId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.User, x => x.Category)
                .Where(x => x.CategoryId == categoryId && !x.IsClosed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Question>> GetByUserIdAsync(Guid userId, bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.Category)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Question>> GetOpenQuestionsAsync(bool asTracked = false, CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(asTracked, x => x.User, x => x.Category)
                .Where(x => !x.IsClosed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        // -----------------------------
        //  Search
        // -----------------------------
        public async Task<IQueryable<Question>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
        {
            keyword = keyword.Trim();

            return  GetQueryWithIncludes(false, x => x.User, x => x.Category)
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
            => await GetQueryWithIncludes(false, x => x.User, x => x.Category)
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Views)
                .Take(count)
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Question>> GetMostViewedAsync(int count, CancellationToken cancellationToken = default)
            => await GetQuery()
                .OrderByDescending(x => x.Views)
                .Take(count)
                .ToListAsync(cancellationToken);

        // -----------------------------
        //  Unanswered
        // -----------------------------
        public async Task<IEnumerable<Question>> GetUnansweredQuestionsAsync(CancellationToken cancellationToken = default)
            => await GetQueryWithIncludes(false, x => x.User, x => x.Category)
                .Where(x => x.AnswersCount == 0 && !x.IsClosed)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);

        // -----------------------------
        // Tag-Based Queries
        // -----------------------------
        public async Task<IQueryable<Question>> GetByTagIdAsync(Guid tagId, CancellationToken cancellationToken = default)
            =>  GetQuery()
                .Where(q => q.QuestionTags.Any(qt => qt.TagId == tagId))
                .OrderByDescending(x => x.CreatedAt);

        public async Task<IEnumerable<Question>> GetByTagIdsAsync(IEnumerable<Guid> tagIds, CancellationToken cancellationToken = default)
        {
            var tagIdsList = tagIds.ToList();

            return await GetQuery()
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

            return await GetQuery()
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

            return  GetQuery()
                .OrderByDescending(q => q.Score);
        }
    }
}