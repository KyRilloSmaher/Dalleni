using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Infrastructure.Persisitanse.Repositories
{
    public class SavedQuestionRepository:Repository<SavedQuestion>, ISavedQuestionsRepository
    {
        public SavedQuestionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SavedQuestion>> GetSavedQuestionsByUserIdAsync(Guid userId, bool Astracked = false)
        {
            var query = Context.SavedQuestions
                                .Include(sq => sq.User)
                                .Include(sq => sq.Question)
                                    .ThenInclude(q => q.Category)
                                .Include(sq => sq.Question)
                                    .ThenInclude(q => q.QuestionTags)
                                        .ThenInclude(qt => qt.Tag)  // If QuestionTag has a Tag navigation
                                .Include(sq => sq.Question)
                                    .ThenInclude(q => q.Comments)
                                        .ThenInclude(c => c.User)    // If you need Comment's User
                                .Include(sq => sq.Question)
                                    .ThenInclude(q => q.Answers)
                                        .ThenInclude(a => a.User)    // If you need Answer's User
                                .Where(sq => sq.UserId == userId);
            if (Astracked)
            {
                return await query.ToListAsync();
            }
            else
            {
                return await query.AsNoTracking().ToListAsync();
            }
        }
        public async Task<bool> IsQuestionSavedByUserAsync(Guid userId, Guid questionId)
        {
            return await Context.SavedQuestions.AnyAsync(sq => sq.UserId == userId && sq.QuestionId == questionId);
        }
    }
}
