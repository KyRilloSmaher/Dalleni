using Dalleni.Domin.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Domin.DomainEvents.Events
{
    public class VoteAnswerDomainEvent : DomainEvent
    {
        public Guid UserId { get; set; }
        public VoteType Type { get; set; }
        public Guid VoteId { get; set; }
        public Guid AnswerId { get; set; }
        public VoteAnswerDomainEvent( Guid userId, Guid voteId , VoteType type , Guid answerId)
        {
            AnswerId = answerId;
            UserId = userId;
            VoteId = voteId;
            Type =type;
        }
    }
}
