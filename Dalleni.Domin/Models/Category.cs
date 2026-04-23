using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a category used to group questions.
    /// </summary>
    public class Category : DomainEntity
    {
        public Category()
        {
            Questions = new List<Question>();
        }

        private Category(string name) : this()
        {
            Id = Guid.NewGuid();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;
        public int QuestionsCount { get; private set; }

        public ICollection<Question> Questions { get; private set; }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        public static Category Create(string name)
        {
            return new Category(name);
        }

        /// <summary>
        /// Updates the category name.
        /// </summary>
        public void Update(string name)
        {
            EnsureNotDeleted();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
            MarkUpdated();
        }
        /// <summary>
        ///  Increments the question count for this category.
        ///  Should be called when a new question is added to this category.
        /// </summary>
        public void IncrementQuestions() => QuestionsCount++;

        /// <summary>
        /// Decrements the question count for this category.
        /// Should be called when a question is removed from this category.
        /// </summary>
        public void DecrementQuestions()
        {
            if (QuestionsCount > 0)
                QuestionsCount--;
        }
    }
}
