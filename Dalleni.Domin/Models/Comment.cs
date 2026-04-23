using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a comment attached to either a question or an answer.
    /// </summary>
    public class Comment : DomainEntity
    {
        public Comment()
        {
        }

        private Comment(string content, Guid userId, Guid? questionId, Guid? answerId)
        {
            Id = Guid.NewGuid();
            Content = DomainGuard.AgainstNullOrWhiteSpace(content, nameof(content));
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));

            if ((questionId.HasValue && answerId.HasValue) || (!questionId.HasValue && !answerId.HasValue))
            {
                throw new DomainException("A comment must belong to exactly one target.");
            }

            QuestionId = questionId;
            AnswerId = answerId;
        }

        public Guid Id { get; private set; }

        public string Content { get; private set; } = string.Empty;

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public Guid? QuestionId { get; private set; }

        public Question? Question { get; private set; }

        public Guid? AnswerId { get; private set; }

        public Answer? Answer { get; private set; }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        public static Comment Create(string content, Guid userId, Guid? questionId = null, Guid? answerId = null)
        {
            return new Comment(content, userId, questionId, answerId);
        }

        /// <summary>
        /// Updates the comment text.
        /// </summary>
        public void Update(string content)
        {
            EnsureNotDeleted();
            Content = DomainGuard.AgainstNullOrWhiteSpace(content, nameof(content));
            MarkUpdated();
        }
    }
}
