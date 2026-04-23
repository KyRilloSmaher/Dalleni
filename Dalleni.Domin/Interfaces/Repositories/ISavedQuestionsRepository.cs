using Dalleni.Domin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Interfaces.Repositories
{
    /// <summary>
    ///  Repository interface for managing saved questions in the application. 
    ///  This interface defines the contract for operations related to saved questions, such as adding, retrieving, updating, and deleting saved questions.
    ///  It extends the generic IRepository interface, which provides basic CRUD operations for the SavedQuestion entity.
    /// </summary>
    public interface ISavedQuestionsRepository : IRepository<SavedQuestion>
    {
        Task<IEnumerable<SavedQuestion>> GetSavedQuestionsByUserIdAsync(Guid userId, bool Astracked =false);
    }
}
