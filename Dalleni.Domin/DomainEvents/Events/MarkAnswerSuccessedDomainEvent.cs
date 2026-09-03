
namespace Dalleni.Domin.DomainEvents.Events
{
    public class MarkAnswerSuccessedDomainEvent : DomainEvent
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; }
        public Guid UserId { get; }

        public MarkAnswerSuccessedDomainEvent(Guid answerId, Guid questionId, Guid userId)
        {
            AnswerId = answerId;
            QuestionId = questionId;
            UserId = userId;
        }
    }
}