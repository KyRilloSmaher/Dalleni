using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.DomainEvents.Events
{
    public class QuestionCreatedDomainEvent:DomainEvent
    {
        public Guid QuestionId { get; }
        public Guid UserId { get; }

        public QuestionCreatedDomainEvent(Guid questionId, Guid userId )
        {
            QuestionId = questionId;
            UserId = userId;
        }
    }
}
