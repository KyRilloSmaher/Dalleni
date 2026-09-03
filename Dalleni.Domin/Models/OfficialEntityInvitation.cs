using Dalleni.Domin.Enums;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents an invitation for a user to join an official entity.
    /// </summary>
    public class OfficialEntityInvitation : DomainEntity
    {
        private OfficialEntityInvitation()
        {
        }

        private OfficialEntityInvitation(
            Guid officialEntityId,
            Guid invitedByUserId,
            string email,
            EntityRole role,
            string tokenHash,
            DateTime expiresAt)
        {
            Id = Guid.NewGuid();

            OfficialEntityId = DomainGuard.AgainstEmpty(
                officialEntityId,
                nameof(officialEntityId));

            InvitedByUserId = DomainGuard.AgainstEmpty(
                invitedByUserId,
                nameof(invitedByUserId));

            Email = DomainGuard.AgainstNullOrWhiteSpace(
                email,
                nameof(email))
                .ToLowerInvariant();

            Role = role;

            TokenHash = DomainGuard.AgainstNullOrWhiteSpace(
                tokenHash,
                nameof(tokenHash));

            ExpiresAt = expiresAt;

            IsAccepted = false;
        }

        public Guid Id { get; private set; }

        public Guid OfficialEntityId { get; private set; }

        public OfficialEntity OfficialEntity { get; private set; } = null!;

        public Guid InvitedByUserId { get; private set; }

        public ApplicationUser InvitedByUser { get; private set; } = null!;

        public string Email { get; private set; } = string.Empty;

        public EntityRole Role { get; private set; }

        /// <summary>
        /// SHA-256 hash of the invitation token.
        /// The raw token is never stored in the database.
        /// </summary>
        public string TokenHash { get; private set; } = string.Empty;

        public DateTime ExpiresAt { get; private set; }

        public bool IsAccepted { get; private set; }

        public bool IsExpired =>
            DateTime.UtcNow >= ExpiresAt;

        public static OfficialEntityInvitation Create(
            Guid officialEntityId,
            Guid invitedByUserId,
            string email,
            EntityRole role,
            string tokenHash,
            DateTime expiresAt)
        {
            if (expiresAt <= DateTime.UtcNow)
            {
                throw new DomainException(
                    "Invitation expiration must be in the future.");
            }

            return new OfficialEntityInvitation(
                officialEntityId,
                invitedByUserId,
                email,
                role,
                tokenHash,
                expiresAt);
        }

        public void Accept()
        {
            if (IsAccepted)
            {
                throw new DomainException(
                    "Invitation has already been accepted.");
            }

            if (IsExpired)
            {
                throw new DomainException(
                    "Invitation has expired.");
            }

            IsAccepted = true;

            MarkUpdated();
        }
    }
}