using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.DomainEvents.Events
{
    
    public class QuestionAnsweredDomainEvent : DomainEvent
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; }
        public Guid UserId { get; }

        public QuestionAnsweredDomainEvent(Guid answerId,Guid questionId, Guid userId)
        {
            AnswerId = answerId;
            QuestionId = questionId;
            UserId = userId;
        }
    }
}
