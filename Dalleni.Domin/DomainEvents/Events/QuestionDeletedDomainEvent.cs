using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.DomainEvents.Events
{

    public class QuestionDeletedDomainEvent : DomainEvent
    {
        public Guid QuestionId { get; }
        public Guid UserId { get; }

        public QuestionDeletedDomainEvent(Guid questionId, Guid userId)
        {
            QuestionId = questionId;
            UserId = userId;
        }
    }
}
