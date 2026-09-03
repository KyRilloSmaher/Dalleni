using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents an official entity that publishes services.
    /// </summary>
    public class OfficialEntity : DomainEntity
    {
        public OfficialEntity()
        {
            Services = new List<Service>();
            Members = new List<OfficialEntityMembership>();
        }

        private OfficialEntity(string name, string description) : this()
        {
            Id = Guid.NewGuid();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
            Description = DomainGuard.AgainstNullOrWhiteSpace(description, nameof(description));
            
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;


        public bool IsVerified { get; private set; }
        public string? LogoUrl { get; set; }
        
        public string? WebsiteUrl { get; set; }

        public ICollection<Service> Services { get; private set; }
        public ICollection<OfficialEntityMembership> Members { get; private set; }

        public static OfficialEntity Create(string name, string description)
        {
            return new OfficialEntity(name, description);
        }

        /// <summary>
        /// Updates the official entity details.
        /// </summary>
        public void Update(string name, string description)
        {
            EnsureNotDeleted();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
            Description = DomainGuard.AgainstNullOrWhiteSpace(description, nameof(description));
            MarkUpdated();
        }

        /// <summary>
        /// Marks the entity as verified.
        /// </summary>
        public void Verify()
        {
            EnsureNotDeleted();
            IsVerified = true;
            MarkUpdated();
        }
    }
}
