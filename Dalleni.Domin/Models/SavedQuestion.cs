using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a question that has been saved by a user for later reference or review. 
    /// This class can be used to store information about the saved question, such as the question text, the date it was saved, and any relevant metadata.
    /// </summary>
    public class SavedQuestion
    {
        /// <summary>
        /// Unique identifier for the saved question
        /// </summary>
        public Guid Id { get; set; }
        ///<summary>
        /// Identifier for the user who saved the question
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Identifier for the question that was saved
        /// </summary>
        public Guid QuestionId { get; set; }
        /// <summary>
        /// Timestamp for when the question was saved
        /// </summary>
        public DateTime SavedAt { get;} = DateTime.UtcNow;

        /// Navigation properties
        
        public ApplicationUser User { get; set; }
        public Question Question { get; set; }


        // Constructors
        public SavedQuestion() { }

        public SavedQuestion(Guid userId, Guid questionId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            QuestionId = questionId;
        }

        public static SavedQuestion Create(Guid userId, Guid questionId)
              => new SavedQuestion(userId, questionId);

    }
}
