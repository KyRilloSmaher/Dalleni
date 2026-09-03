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
            Ratings= new List<Rating>();
        }

        private Service(
            string name, 
            string description, 
            string requiredDocuments, 
            decimal? fees, 
            Guid categoryId,
            Guid officialEntityId)
        {
            Id = Guid.NewGuid();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
            Description = DomainGuard.AgainstNullOrWhiteSpace(description, nameof(description));
            RequiredDocuments = DomainGuard.AgainstNullOrWhiteSpace(requiredDocuments, nameof(requiredDocuments));
            Fees = DomainGuard.AgainstNegative(fees, nameof(fees));
            CategoryId = categoryId;
            IsAvailable = true;
            OfficialEntityId = DomainGuard.AgainstEmpty(officialEntityId, nameof(officialEntityId));
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public string RequiredDocuments { get; private set; } = string.Empty;

        public decimal? Fees { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category {get;set;}

        public bool IsAvailable { get; private set; }

        public int RatingCount { get; private set; }

        public double AverageRating { get; private set; }
        public Guid OfficialEntityId { get; private set; }

        public OfficialEntity OfficialEntity { get; private set; } = null!;

        public ICollection<Rating> Ratings { get; private set; } = new List<Rating>();

        public static Service Create(
            string name, 
            string description, 
            string requiredDocuments, 
            decimal? fees, 
            Guid categoryId,
            Guid officialEntityId)
        {
            return new Service(name, description, requiredDocuments, fees, categoryId, officialEntityId);
        }

        /// <summary>
        /// Updates the service information.
        /// </summary>
        public void Update(
            string name, 
            string description, 
            string requiredDocuments, 
            decimal? fees, 
            Guid category)
        {
            EnsureNotDeleted();
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));
            Description = DomainGuard.AgainstNullOrWhiteSpace(description, nameof(description));
            RequiredDocuments = DomainGuard.AgainstNullOrWhiteSpace(requiredDocuments, nameof(requiredDocuments));
            Fees = DomainGuard.AgainstNegative(fees, nameof(fees));
            CategoryId = category;
            MarkUpdated();
        }

        /// <summary>
        /// Sets the availability status of the service.
        /// </summary>
        public void ToggleAvailability()
        {
            EnsureNotDeleted();
            IsAvailable = ! IsAvailable;
            MarkUpdated();
        }

        /// <summary>
        /// Calculates the average rating from the ratings collection.
        /// </summary>
        public double GetAverageRating()
        {

             return Ratings.Any() ? Ratings.Average(r => r.Value) : 0.0;
        
        }

        /// <summary>
        /// Gets the official entity name from the navigation property.
        /// </summary>
        public string GetOfficialEntityName()
        {
            return OfficialEntity?.Name ?? string.Empty;
        }

        /// <summary>
        /// Gets the official entity verification status.
        /// </summary>
        public bool IsOfficialEntityVerified()
        {
            return OfficialEntity?.IsVerified ?? false;
        }

        /// <summary>
        /// Updates the average rating of the service. This method can be called after a new rating is added or an existing rating is updated.
        /// </summary>
        /// <param name="newAverage"></param>
        public void UpdateAverageRating(double newAverage)
        {
            AverageRating = newAverage;
            MarkUpdated();
        }

        public void AddRating(double value)
        {
            EnsureNotDeleted();

            var totalRating = AverageRating * RatingCount;

            RatingCount++;

            AverageRating = (totalRating + value) / RatingCount;

            MarkUpdated();
        }

        public void UpdateRating(double oldValue, double newValue)
        {
            EnsureNotDeleted();

            if (RatingCount <= 0)
                return;

            var totalRating = AverageRating * RatingCount;

            totalRating -= oldValue;
            totalRating += newValue;

            AverageRating = totalRating / RatingCount;

            MarkUpdated();
        }

        public void RemoveRating(double value)
        {
            EnsureNotDeleted();

            if (RatingCount <= 0)
                return;

            var totalRating = AverageRating * RatingCount;

            totalRating -= value;
            RatingCount--;

            AverageRating = RatingCount > 0
                ? totalRating / RatingCount
                : 0.0;

            MarkUpdated();
        }


    }
}