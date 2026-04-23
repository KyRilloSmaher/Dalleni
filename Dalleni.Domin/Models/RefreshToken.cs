using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a refresh token issued to a user.
    /// </summary>
    public class RefreshToken : DomainEntity
    {
        public RefreshToken()
        {
        }

        private RefreshToken(string token, DateTime expiresAt, Guid userId)
        {
            Id = Guid.NewGuid();
            Token = DomainGuard.AgainstNullOrWhiteSpace(token, nameof(token));
            ExpiresAt = DomainGuard.AgainstPast(expiresAt, nameof(expiresAt));
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));
        }

        public Guid Id { get; private set; }

        public string Token { get; private set; } = string.Empty;

        public DateTime ExpiresAt { get; private set; }

        public DateTime? RevokedAt { get; private set; }

        public bool IsRevoked => RevokedAt.HasValue;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public static RefreshToken Create(string token, DateTime expiresAt, Guid userId)
        {
            return new RefreshToken(token, expiresAt, userId);
        }

        /// <summary>
        /// Revokes the refresh token.
        /// </summary>
        public void Revoke()
        {
            EnsureNotDeleted();

            if (IsRevoked)
            {
                throw new DomainException("The refresh token has already been revoked.");
            }

            RevokedAt = DateTime.UtcNow;
            MarkUpdated();
        }

        /// <summary>
        /// Replaces the current token value and expiration date.
        /// </summary>
        public void Renew(string token, DateTime expiresAt)
        {
            EnsureNotDeleted();
            Token = DomainGuard.AgainstNullOrWhiteSpace(token, nameof(token));
            ExpiresAt = DomainGuard.AgainstPast(expiresAt, nameof(expiresAt));
            RevokedAt = null;
            MarkUpdated();
        }
    }
}
