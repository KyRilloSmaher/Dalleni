using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Models;
using Microsoft.AspNetCore.Identity;

namespace Dalleni.Domin.Models
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Questions = new List<Question>();
            Answers = new List<Answer>();
            Comments = new List<Comment>();
            Votes = new List<Vote>();
            RefreshTokens = new List<RefreshToken>();
            ExternalLogins = new List<ExternalLogin>();
            OfficialEntities = new List<OfficialEntity>();
            SecurityStamp = Guid.NewGuid().ToString("N");
        }

        private ApplicationUser(string fullName, string email, string userName, string? phoneNumber, string? profileImageUrl) : this()
        {
            FullName = RequireValue(fullName, nameof(fullName));
            Email = RequireValue(email, nameof(email));
            NormalizedEmail = Email.ToUpperInvariant();
            UserName = RequireValue(userName, nameof(userName));
            NormalizedUserName = UserName.ToUpperInvariant();
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
            ProfileImageUrl = string.IsNullOrWhiteSpace(profileImageUrl) ? null : profileImageUrl.Trim();
        }

        // -----------------------------
        // Core Profile
        // -----------------------------
        public string FullName { get; private set; } = string.Empty;
        public int Reputation { get; private set; }
        public DateTime? LastLoginAt { get; private set; }
        public string? ProfileImageUrl { get; set; }

        // -----------------------------
        // User Statistics
        // -----------------------------
        public int QuestionsCount { get; private set; }
        public int AnswersCount { get; private set; }
        public int AcceptedAnswersCount { get; private set; }
        public int TotalUpVotesReceived { get; private set; }
        public int TotalDownVotesReceived { get; private set; }

        // -----------------------------
        // Security / Auth
        // -----------------------------
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPExpiryTime { get; set; }
        public int OTPAttempts { get; set; }
        public bool ResetPasswordConfirmed { get; set; }

        public bool IsDeleted { get; private set; }

        // -----------------------------
        // Navigation
        // -----------------------------
        public ICollection<Question> Questions { get; private set; }
        public ICollection<Answer> Answers { get; private set; }
        public ICollection<Comment> Comments { get; private set; }
        public ICollection<Vote> Votes { get; private set; }
        public ICollection<RefreshToken> RefreshTokens { get; private set; }
        public ICollection<ExternalLogin> ExternalLogins { get; private set; }
        public ICollection<OfficialEntity> OfficialEntities { get; private set; }
        public ICollection<SavedQuestion> SavedQuestions { get; private set; }

        // -----------------------------
        // Factory
        // -----------------------------
        public static ApplicationUser Create(string fullName, string email, string userName, string? phoneNumber = null, string? profileImageUrl = null)
            => new(fullName, email, userName, phoneNumber, profileImageUrl);

        // -----------------------------
        // Profile
        // -----------------------------
        public void UpdateProfile(string fullName, string? profileImageUrl)
        {
            FullName = RequireValue(fullName, nameof(fullName));
            ProfileImageUrl = string.IsNullOrWhiteSpace(profileImageUrl) ? null : profileImageUrl.Trim();
        }

        // -----------------------------
        // 🔥 Statistics Management (DDD SAFE)
        // -----------------------------
        public void OnQuestionCreated()
        {
            QuestionsCount++;
        }

        public void OnQuestionDeleted()
        {
            if (QuestionsCount > 0)
                QuestionsCount--;
        }

        public void OnAnswerCreated()
        {
            AnswersCount++;
        }

        public void OnAnswerDeleted()
        {
            if (AnswersCount > 0)
                AnswersCount--;
        }

        public void OnAnswerAccepted()
        {
            AcceptedAnswersCount++;
        }

        public void OnAnswerUnaccepted()
        {
            if (AcceptedAnswersCount > 0)
                AcceptedAnswersCount--;
        }

        public void OnReceiveUpVote()
        {
            TotalUpVotesReceived++;
            AdjustReputation(+10); 
        }

        public void OnReceiveDownVote()
        {
            TotalDownVotesReceived++;
            AdjustReputation(-2);
        }

        // -----------------------------
        // Lifecycle
        // -----------------------------
        public virtual void Delete()
        {
            if (IsDeleted)
                throw new DomainException("The entity has already been deleted.");

            IsDeleted = true;
        }

        public virtual void Restore()
        {
            if (!IsDeleted)
                throw new DomainException("The entity is already active.");

            IsDeleted = false;
        }

        public void RecordLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void AdjustReputation(int delta)
        {
            var updatedReputation = Reputation + delta;
            if (updatedReputation < 0)
                throw new DomainException("User reputation cannot be negative.");

            Reputation = updatedReputation;
        }

        private static string RequireValue(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{paramName} is required.");

            return value.Trim();
        }
    }
}