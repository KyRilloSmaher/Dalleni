using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents an external login linked to a user account.
    /// </summary>
    public class ExternalLogin : DomainEntity
    {
        public ExternalLogin()
        {
        }

        private ExternalLogin(string provider, string providerKey, Guid userId, string? email, string? displayName, string? profilePictureUrl)
        {
            Id = Guid.NewGuid();
            Provider = DomainGuard.AgainstNullOrWhiteSpace(provider, nameof(provider));
            ProviderKey = DomainGuard.AgainstNullOrWhiteSpace(providerKey, nameof(providerKey));
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim();
            ProfilePictureUrl = string.IsNullOrWhiteSpace(profilePictureUrl) ? null : profilePictureUrl.Trim();
        }

        public Guid Id { get; private set; }

        public string Provider { get; private set; } = string.Empty;

        public string ProviderKey { get; private set; } = string.Empty;

        public string? Email { get; private set; }

        public string? DisplayName { get; private set; }

        public string? ProfilePictureUrl { get; private set; }

        public DateTime? LastUsedAt { get; private set; }

        public Guid UserId { get; private set; }

        public ApplicationUser User { get; private set; } = null!;

        public static ExternalLogin Create(string provider, string providerKey, Guid userId, string? email = null, string? displayName = null, string? profilePictureUrl = null)
        {
            return new ExternalLogin(provider, providerKey, userId, email, displayName, profilePictureUrl);
        }

        /// <summary>
        /// Updates the external login profile data.
        /// </summary>
        public void UpdateProfile(string? email, string? displayName, string? profilePictureUrl)
        {
            EnsureNotDeleted();
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim();
            ProfilePictureUrl = string.IsNullOrWhiteSpace(profilePictureUrl) ? null : profilePictureUrl.Trim();
            MarkUpdated();
        }

        /// <summary>
        /// Tracks the last successful usage time.
        /// </summary>
        public void RecordUsage()
        {
            EnsureNotDeleted();
            LastUsedAt = DateTime.UtcNow;
            MarkUpdated();
        }
    }
}
