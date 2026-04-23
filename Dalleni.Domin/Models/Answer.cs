using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;
using System.Runtime.CompilerServices;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents an answer submitted for a question.
    /// </summary>
    public class Answer : DomainEntity
    {
        public Answer()
        {
            Comments = new List<Comment>();
            Votes = new List<Vote>();
        }

        private Answer(string content, Guid questionId, Guid userId) : this()
        {
            Id = Guid.NewGuid();
            Content = DomainGuard.AgainstNullOrWhiteSpace(content, nameof(content));
            QuestionId = DomainGuard.AgainstEmpty(questionId, nameof(questionId));
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));

            CreatedAt = DateTime.UtcNow;
            Score = 0;
        }

        public Guid Id { get; private set; }

        public string Content { get; private set; } = string.Empty;

        public Guid QuestionId { get; private set; }

        public Question Question { get; private set; } = null!;

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public bool IsAccepted { get; private set; }

        public int UpVotes { get; private set; }

        public int DownVotes { get; private set; }

        public int SuccessCount { get; private set; }

        /// <summary>
        /// Base score (without time decay)
        /// </summary>
        public double Score { get; private set; }

        public ICollection<Comment> Comments { get; private set; }

        public ICollection<Vote> Votes { get; private set; }

        /// <summary>
        /// Creates a new answer.
        /// </summary>
        public static Answer Create(string content, Guid questionId, Guid userId)
        {
            var answer =  new Answer(content, questionId, userId);
            answer.RaiseDomainEvent(new QuestionAnsweredDomainEvent(answer.Id,questionId, userId));
            return answer;
        }

        /// <summary>
        /// Updates the answer content.
        /// </summary>
        public void Update(string content)
        {
            EnsureNotDeleted();

            Content = DomainGuard.AgainstNullOrWhiteSpace(content, nameof(content));

            MarkUpdated();
        }

        /// <summary>
        /// Marks the answer as accepted.
        /// </summary>
        public void Accept()
        {
            EnsureNotDeleted();

            if (IsAccepted) return;

            IsAccepted = true;
            SuccessCount++;
            Score += 20;

            MarkUpdated();
        }

        /// <summary>
        /// Removes the accepted state from the answer.
        /// </summary>
        public void Unaccept()
        {
            EnsureNotDeleted();

            if (!IsAccepted) return;

            IsAccepted = false;
            Score -= 20;

            MarkUpdated();
        }

        /// <summary>
        /// Called when user confirms this answer worked for them.
        /// </summary>
        public void MarkAsSuccessful()
        {
            EnsureNotDeleted();

            SuccessCount++;

            Score += 5;

            MarkUpdated();
        }

        /// <summary>
        /// Applies a vote using incremental scoring.
        /// </summary>
        public void ApplyVote(VoteType voteType)
        {
            EnsureNotDeleted();

            if (voteType == VoteType.Upvote)
            {
                UpVotes++;
                Score += 2;
            }
            else
            {
                DownVotes++;
                Score -= 1;
            }

            MarkUpdated();
        }

        /// <summary>
        /// Gets the final score including time decay (not stored).
        /// </summary>
        public double GetFinalScore()
        {
            var ageInHours = (DateTime.UtcNow - CreatedAt).TotalHours;

            Score -= (ageInHours * 0.1);
            return Score;
        }

        ///<summary>
        /// 
        /// </summary>
        
        public void RaiseDeleteEvent()
        {
            RaiseDomainEvent(new QuestionAnsweredDeletedDomainEvent(Id, QuestionId, UserId));
        }
    }
}