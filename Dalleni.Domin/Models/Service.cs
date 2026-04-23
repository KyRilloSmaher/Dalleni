using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a service published by an official entity.
    /// </summary>
    public class Service : DomainEntity
    {
        public Service()
        {
        }

        private Service(string name, string description, string requiredDocuments, decimal? fees, Guid officialEntityId)
        {
            Id = Guid.NewGuid();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
            Description = DomainGuard.AgainstNullOrWhiteSpace(description, nameof(description));
            RequiredDocuments = DomainGuard.AgainstNullOrWhiteSpace(requiredDocuments, nameof(requiredDocuments));
            Fees = DomainGuard.AgainstNegative(fees, nameof(fees));
            OfficialEntityId = DomainGuard.AgainstEmpty(officialEntityId, nameof(officialEntityId));
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public string RequiredDocuments { get; private set; } = string.Empty;

        public decimal? Fees { get; private set; }

        public Guid OfficialEntityId { get; private set; }

        public OfficialEntity OfficialEntity { get; private set; } = null!;

        public static Service Create(string name, string description, string requiredDocuments, decimal? fees, Guid officialEntityId)
        {
            return new Service(name, description, requiredDocuments, fees, officialEntityId);
        }

        /// <summary>
        /// Updates the service information.
        /// </summary>
        public void Update(string name, string description, string requiredDocuments, decimal? fees)
        {
            EnsureNotDeleted();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
            Description = DomainGuard.AgainstNullOrWhiteSpace(description, nameof(description));
            RequiredDocuments = DomainGuard.AgainstNullOrWhiteSpace(requiredDocuments, nameof(requiredDocuments));
            Fees = DomainGuard.AgainstNegative(fees, nameof(fees));
            MarkUpdated();
        }
    }
}
