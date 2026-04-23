using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents the association between a question and a tag.
    /// </summary>
    public class QuestionTag : DomainEntity
    {
        private QuestionTag(Guid questionId, Guid tagId)
        {
            QuestionId = DomainGuard.AgainstEmpty(questionId, nameof(questionId));
            TagId = DomainGuard.AgainstEmpty(tagId, nameof(tagId));
        }

        public Guid QuestionId { get; private set; }

        public Question Question { get; private set; } = null!;

        public Guid TagId { get; private set; }

        public Tag Tag { get; private set; } = null!;

        public static QuestionTag Create(Guid questionId, Guid tagId)
        {
            return new QuestionTag(questionId, tagId);
        }
    }
}