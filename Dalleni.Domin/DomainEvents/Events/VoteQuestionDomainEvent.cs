using Dalleni.Domin.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.DomainEvents.Events
{
    public class VoteQuestionDomainEvent:DomainEvent
    {
        public Guid UserId { get; set; }
        public VoteType type { get; set; }
        public Guid VoteId { get; set; }
        public Guid QuestionId { get; set; }
        public VoteQuestionDomainEvent(Guid userId, Guid voteId, VoteType Type, Guid questionId)
        {
            type = Type;
            UserId = userId;
            VoteId = voteId;
            QuestionId = questionId;
        }
    }
}
