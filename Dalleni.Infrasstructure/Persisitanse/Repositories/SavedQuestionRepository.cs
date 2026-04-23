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
            var query = Context.SavedQuestions.Where(sq => sq.UserId == userId);
            if (Astracked)
            {
                return await query.ToListAsync();
            }
            else
            {
                return await query.AsNoTracking().ToListAsync();
            }
        }
    }
}
