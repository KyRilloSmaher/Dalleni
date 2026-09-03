using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents an answer submitted for a question.
    ///
    /// An answer can be either:
    /// - Community: submitted by a normal user and participates in voting/scoring.
    /// - Official: submitted on behalf of an official entity and does not
    ///   participate in voting or community scoring.
    /// </summary>
    public class Answer : DomainEntity
    {


        public Answer()
        {
            Comments = new List<Comment>();
            Votes = new List<Vote>();
        }


        private Answer(
            string content,
            Guid questionId,
            Guid userId,
            AnswerType type,
            Guid? officialEntityId) : this()
        {
            Id = Guid.NewGuid();

            Content = DomainGuard.AgainstNullOrWhiteSpace(
                content,
                nameof(content));

            QuestionId = DomainGuard.AgainstEmpty(
                questionId,
                nameof(questionId));

            UserId = DomainGuard.AgainstEmpty(
                userId,
                nameof(userId));

            Type = type;

            // -----------------------------------------------------
            // Official Answer Validation
            // -----------------------------------------------------

            if (type == AnswerType.Official)
            {
                if (!officialEntityId.HasValue ||officialEntityId.Value == Guid.Empty)
                {
                    throw new DomainException(
                        "An official answer must belong to an official entity.");
                }
            }

            // -----------------------------------------------------
            // Community Answer Validation
            // -----------------------------------------------------

            if (type == AnswerType.Community)
            {
                if (officialEntityId.HasValue)
                {
                    throw new DomainException(
                        "A community answer cannot belong to an official entity.");
                }
            }

            OfficialEntityId = officialEntityId;

            CreatedAt = DateTime.UtcNow;

            IsAccepted = false;

            UpVotes = 0;
            DownVotes = 0;
            SuccessCount = 0;
            Score = 0;
        }


        public Guid Id { get; private set; }
        public string Content { get; private set; } = string.Empty;

        public Guid QuestionId { get; private set; }

        public Question Question { get; private set; } = null!;

        /// <summary>
        /// The actual application user who submitted the answer.
        ///
        /// For an official answer, this represents the employee/account
        /// that performed the action on behalf of the official entity.
        /// </summary>
        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;


        public AnswerType Type { get; private set; }

        // ---------------------------------------------------------
        // Official Entity
        // ---------------------------------------------------------

        /// <summary>
        /// The official entity represented by this answer.
        ///
        /// Null for community answers.
        /// </summary>
        public Guid? OfficialEntityId { get; private set; }

        public OfficialEntity? OfficialEntity { get; private set; }


        /// <summary>
        /// Indicates whether the answer was accepted by the
        /// user who asked the question.
        /// </summary>
        public bool IsAccepted { get; private set; }

        // ---------------------------------------------------------
        // Community Statistics
        // ---------------------------------------------------------

        /// <summary>
        /// Number of upvotes.
        ///
        /// Official answers do not participate in voting.
        /// </summary>
        public int UpVotes { get; private set; }

        /// <summary>
        /// Number of downvotes.
        ///
        /// Official answers do not participate in voting.
        /// </summary>
        public int DownVotes { get; private set; }

        /// <summary>
        /// Number of users who confirmed that the answer was successful.
        ///
        /// This is maintained independently from voting.
        /// </summary>
        public int SuccessCount { get; private set; }

        /// <summary>
        /// Community score.
        ///
        /// Official answers do not receive score changes.
        /// </summary>
        public double Score { get; private set; }


        public ICollection<Comment> Comments { get; private set; }

        public ICollection<Vote> Votes { get; private set; }

        // =========================================================
        // Factory Methods
        // =========================================================

        /// <summary>
        /// Creates a normal community answer.
        /// </summary>
        public static Answer CreateCommunityAnswer(
            string content,
            Guid questionId,
            Guid userId)
        {
            var answer = new Answer(
                content,
                questionId,
                userId,
                AnswerType.Community,
                null);

            answer.RaiseDomainEvent(
                new QuestionAnsweredDomainEvent(
                    answer.Id,
                    questionId,
                    userId));

            return answer;
        }

        /// <summary>
        /// Creates an official answer on behalf of an official entity.
        ///
        /// Authorization that the user is actually a member of the
        /// official entity should be handled before calling this method.
        /// </summary>
        public static Answer CreateOfficialAnswer(
            string content,
            Guid questionId,
            Guid userId,
            Guid officialEntityId)
        {
            var answer = new Answer(
                content,
                questionId,
                userId,
                AnswerType.Official,
                officialEntityId);

            answer.RaiseDomainEvent(
                new QuestionAnsweredDomainEvent(
                    answer.Id,
                    questionId,
                    userId));

            return answer;
        }

        // =========================================================
        // Content Management
        // =========================================================

        /// <summary>
        /// Updates the answer content.
        /// </summary>
        public void Update(string content)
        {
            EnsureNotDeleted();

            Content = DomainGuard.AgainstNullOrWhiteSpace(
                content,
                nameof(content));

            MarkUpdated();
        }

        // =========================================================
        // Acceptance
        // =========================================================

        /// <summary>
        /// Marks this answer as accepted.
        ///
        /// Important:
        /// The domain entity itself does not know who is performing
        /// the action. The application layer must verify that the
        /// current user is the author of the question before calling
        /// this method.
        ///
        /// Both community and official answers can be accepted.
        ///
        /// Acceptance does NOT modify score or reputation.
        /// </summary>
        public void Accept()
        {
            EnsureNotDeleted();

            if (IsAccepted)
                return;

            IsAccepted = true;

            MarkUpdated();
            RaiseDomainEvent(
                new QuestionAnsweredAcceptedDomainEvent(
                    Id,
                    QuestionId,
                    UserId));
            
        }

        /// <summary>
        /// Removes the accepted state from this answer.
        ///
        /// Both community and official answers can be unaccepted.
        ///
        /// Unaccepting does NOT modify score or reputation.
        /// </summary>
        public void Unaccept()
        {
            EnsureNotDeleted();

            if (!IsAccepted)
                return;

            IsAccepted = false;

            MarkUpdated();
        }

        // =========================================================
        // Success
        // =========================================================

        /// <summary>
        /// Marks the answer as successful.
        ///
        /// Only community answers participate in success scoring.
        /// Official answers can still be considered successful by
        /// the application without affecting community score.
        /// </summary>
        public void MarkAsSuccessful()
        {
            EnsureNotDeleted();

            SuccessCount++;

            if (Type == AnswerType.Community)
            {
                Score += 5;
            }

            MarkUpdated();
        }

        /// <summary>
        /// Removes a successful confirmation.
        /// </summary>
        public void UnmarkAsSuccessful()
        {
            EnsureNotDeleted();

            if (SuccessCount <= 0)
                return;

            SuccessCount--;

            if (Type == AnswerType.Community)
            {
                Score -= 5;
            }

            MarkUpdated();
        }

        // =========================================================
        // Voting
        // =========================================================

        /// <summary>
        /// Applies a vote to a community answer.
        ///
        /// Official answers cannot be voted on.
        /// </summary>
        public void ApplyVote(VoteType voteType)
        {
            EnsureNotDeleted();

            EnsureCommunityAnswer();

            if (voteType == VoteType.Upvote)
            {
                UpVotes++;
                Score += 2;
            }
            else if (voteType == VoteType.Downvote)
            {
                DownVotes++;
                Score -= 1;
            }
            else
            {
                throw new DomainException(
                    "Unsupported vote type.");
            }

            MarkUpdated();
        }

        /// <summary>
        /// Removes a vote from a community answer.
        ///
        /// Official answers cannot have votes removed because they
        /// cannot receive votes in the first place.
        /// </summary>
        public bool DecreaseVote(VoteType voteType)
        {
            EnsureNotDeleted();

            EnsureCommunityAnswer();

            if (voteType == VoteType.Upvote && UpVotes > 0)
            {
                UpVotes--;
                Score -= 2;

                MarkUpdated();

                return true;
            }

            if (voteType == VoteType.Downvote && DownVotes > 0)
            {
                DownVotes--;
                Score += 1;

                MarkUpdated();

                return true;
            }

            return false;
        }

        // =========================================================
        // Score
        // =========================================================

        /// <summary>
        /// Gets the final community ranking score including time decay.
        ///
        /// Official answers do not participate in community scoring.
        /// </summary>
        public double GetFinalScore()
        {
            EnsureNotDeleted();

            if (Type == AnswerType.Official)
                return 0;

            var ageInHours =
                (DateTime.UtcNow - CreatedAt).TotalHours;

            return Score - (ageInHours * 0.1);
        }

        // =========================================================
        // Domain Events
        // =========================================================

        /// <summary>
        /// Raises the domain event used when an answer is deleted.
        /// </summary>
        public void RaiseDeleteEvent()
        {
            RaiseDomainEvent(
                new QuestionAnsweredDeletedDomainEvent(
                    Id,
                    QuestionId,
                    UserId));
        }

        // =========================================================
        // Private Helpers
        // =========================================================

        private void EnsureCommunityAnswer()
        {
            if (Type == AnswerType.Official)
            {
                throw new DomainException(
                    "Official answers do not support voting or community scoring.");
            }
        }
    }
}