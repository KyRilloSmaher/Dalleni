using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a vote cast on a question or an answer.
    /// </summary>
    public class Vote : DomainEntity
    {
        public Vote()
        {
        }

        private Vote(Guid userId, VoteType type, Guid? questionId, Guid? answerId)
        {
            Id = Guid.NewGuid();
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));

            if ((questionId.HasValue && answerId.HasValue) || (!questionId.HasValue && !answerId.HasValue))
            {
                throw new DomainException("A vote must target exactly one item.");
            }

            QuestionId = questionId;
            AnswerId = answerId;
            Type = type;
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public Guid? QuestionId { get; private set; }

        public Question? Question { get; private set; }

        public Guid? AnswerId { get; private set; }

        public Answer? Answer { get; private set; }

        public VoteType Type { get; private set; }

        public static Vote Create(Guid userId, VoteType type, Guid? questionId = null, Guid? answerId = null)
        {
            var vote =  new Vote(userId, type, questionId, answerId);
            if (answerId != null)
            {
                vote.RaiseDomainEvent(new VoteAnswerDomainEvent(userId, vote.Id, vote.Type, (Guid)answerId));
            }
            else if (questionId != null)
            {
                vote.RaiseDomainEvent(new VoteQuestionDomainEvent(userId, vote.Id, vote.Type, (Guid)questionId));
            }
                return vote;
        }

        /// <summary>
        /// Changes the vote type.
        /// </summary>
        public void UpdateType(VoteType type)
        {
            EnsureNotDeleted();
            Type = type;
            MarkUpdated();
        }
    }
}
