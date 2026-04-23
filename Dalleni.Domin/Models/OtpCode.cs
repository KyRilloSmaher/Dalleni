using Dalleni.Domin.Enums;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a one-time password code for a user workflow.
    /// </summary>
    public class OtpCode : DomainEntity
    {
        public OtpCode()
        {
        }

        private OtpCode(string code, OtpType purpose, DateTime expiresAt, Guid userId)
        {
            Id = Guid.NewGuid();
            Code = DomainGuard.AgainstNullOrWhiteSpace(code, nameof(code));
            Purpose = purpose;
            ExpiresAt = DomainGuard.AgainstPast(expiresAt, nameof(expiresAt));
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));
        }

        public Guid Id { get; private set; }

        public string Code { get; private set; } = string.Empty;

        public OtpType Purpose { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public bool IsUsed { get; private set; }

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public static OtpCode Create(string code, OtpType purpose, DateTime expiresAt, Guid userId)
        {
            return new OtpCode(code, purpose, expiresAt, userId);
        }

        /// <summary>
        /// Marks the OTP as used.
        /// </summary>
        public void MarkAsUsed()
        {
            EnsureNotDeleted();

            if (IsUsed)
            {
                throw new DomainException("The OTP code has already been used.");
            }

            if (DateTime.UtcNow > ExpiresAt)
            {
                throw new DomainException("The OTP code has expired.");
            }

            IsUsed = true;
            MarkUpdated();
        }
    }
}
