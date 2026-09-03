
namespace Dalleni.Domin.DomainEvents.Events
{
    public class QuestionAnsweredAcceptedDomainEvent : DomainEvent
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; }
        public Guid UserId { get; }

        public QuestionAnsweredAcceptedDomainEvent(Guid answerId, Guid questionId, Guid userId)
        {
            AnswerId = answerId;
            QuestionId = questionId;
            UserId = userId;
        }
    }
}