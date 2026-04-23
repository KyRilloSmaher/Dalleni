using Dalleni.Domin.DomainEvents.Events;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a question posted by a user.
    /// </summary>
    public class Question : DomainEntity
    {
        private const int MaxTags = 5;

        public Question()
        {
            Answers = new List<Answer>();
            Comments = new List<Comment>();
            QuestionTags = new List<QuestionTag>();
        }

        private Question(string title, string content, Guid userId, Guid categoryId) : this()
        {
            Id = Guid.NewGuid();
            Title = DomainGuard.AgainstNullOrWhiteSpace(title, nameof(title));
            Content = DomainGuard.AgainstNullOrWhiteSpace(content, nameof(content));
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));
            CategoryId = DomainGuard.AgainstEmpty(categoryId, nameof(categoryId));

            CreatedAt = DateTime.UtcNow;
            Score = 0;
        }

        public Guid Id { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public string Content { get; private set; } = string.Empty;

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; } = null!;

        public Guid? AcceptedAnswerId { get; private set; }

        public int UpVotes { get; private set; }

        public int DownVotes { get; private set; }

        public int Views { get; private set; }

        public int AnswersCount { get; private set; }

        public double Score { get; private set; }

        public bool IsClosed { get; private set; }

        public ICollection<Answer> Answers { get; private set; }

        public ICollection<Comment> Comments { get; private set; }

        public ICollection<QuestionTag> QuestionTags { get; private set; }
        public ICollection<SavedQuestion> SavedQuestions { get; private set; }

        public static Question Create(string title, string content, Guid userId, Guid categoryId)
        {
            var  question =  new Question(title, content, userId, categoryId);
            question.RaiseDomainEvent(new QuestionCreatedDomainEvent(question.Id , userId));
            return question;
        }

        public void Update(string title, string content, Guid categoryId)
        {
            EnsureNotDeleted();

            Title = DomainGuard.AgainstNullOrWhiteSpace(title, nameof(title));
            Content = DomainGuard.AgainstNullOrWhiteSpace(content, nameof(content));
            CategoryId = DomainGuard.AgainstEmpty(categoryId, nameof(categoryId));

            MarkUpdated();
        }
        public void RaiseDelete()
        {
            this.RaiseDomainEvent(new QuestionDeletedDomainEvent(this.Id,this.UserId));
        }

        // -----------------------------
        // Tag Management (DDD Control)
        // -----------------------------
        public void AddTag(Tag tag)
        {
            EnsureNotDeleted();

            if (tag == null || tag.IsDeleted)
                throw new DomainException("Invalid tag.");

            if (QuestionTags.Any(qt => qt.TagId == tag.Id))
                return;

            if (QuestionTags.Count >= MaxTags)
                throw new DomainException($"Maximum {MaxTags} tags allowed.");

            QuestionTags.Add(QuestionTag.Create(Id, tag.Id));
            tag.IncrementUsage();
        }

        public void RemoveTag(Guid tagId)
        {
            EnsureNotDeleted();

            var qt = QuestionTags.FirstOrDefault(x => x.TagId == tagId);
            if (qt == null) return;

            QuestionTags.Remove(qt);
        }

        // -----------------------------
        // Engagement Logic
        // -----------------------------
        public void RegisterView()
        {
            EnsureNotDeleted();

            Views++;
            Score += 0.01;

            MarkUpdated();
        }

        public void ApplyVote(VoteType voteType)
        {
            EnsureNotDeleted();

            if (voteType == VoteType.Upvote)
            {
                UpVotes++;
                Score += 1.5;
            }
            else
            {
                DownVotes++;
                Score -= 1.0;
            }

            MarkUpdated();
        }

        public void OnAnswerAdded()
        {
            EnsureNotDeleted();

            AnswersCount++;
            Score += 2;

            MarkUpdated();
        }

        public void OnAnswerRemoved()
        {
            EnsureNotDeleted();

            if (AnswersCount > 0)
                AnswersCount--;

            Score -= 2;
            MarkUpdated();
        }

        public void AcceptAnswer(Guid answerId)
        {
            EnsureNotDeleted();

            AcceptedAnswerId = DomainGuard.AgainstEmpty(answerId, nameof(answerId));
            Score += 5;

            MarkUpdated();
        }

        public void Close()
        {
            EnsureNotDeleted();
            IsClosed = true;
            MarkUpdated();
        }

        public void Reopen()
        {
            EnsureNotDeleted();
            IsClosed = false;
            MarkUpdated();
        }

        // -----------------------------
        // Ranking Algorithm
        // -----------------------------
        public double GetHotScore()
        {
            var ageInHours = (DateTime.UtcNow - CreatedAt).TotalHours;
            this.Score = Score / Math.Pow((ageInHours + 2), 1.5);
            return Score;
        }
    }
}