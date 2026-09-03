using Dalleni.Domin.Helpers;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a rating/review given by a user to a service.
    /// </summary>
    public class Rating : DomainEntity
    {
        public Rating()
        {
        }

        private Rating(
            Guid serviceId,
            Guid userId,
            int value,
            string? comment,
            string? userName = null)
        {
            Id = Guid.NewGuid();
            ServiceId = DomainGuard.AgainstEmpty(serviceId, nameof(serviceId));
            UserId = DomainGuard.AgainstEmpty(userId, nameof(userId));
            Value = ValidateRatingValue(value);
            Comment = comment;
            UserName = userName;
        }

        public Guid Id { get; private set; }

        public Guid ServiceId { get; private set; }

        public Service Service { get; private set; } = null!;

        public Guid UserId { get; private set; }
        public ApplicationUser User {get ; private set;} = null!;

        public int Value { get; private set; }

        public string? Comment { get; private set; }

        public string? UserName { get; private set; }

        public static Rating Create(
            Guid serviceId,
            Guid userId,
            int value,
            string? comment = null,
            string? userName = null)
        {
            return new Rating(serviceId, userId, value, comment, userName);
        }

        /// <summary>
        /// Updates the rating value and comment.
        /// </summary>
        public void Update(int value, string? comment)
        {
            EnsureNotDeleted();
            Value = ValidateRatingValue(value);
            Comment = comment;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the comment only.
        /// </summary>
        public void UpdateComment(string? comment)
        {
            EnsureNotDeleted();
            Comment = comment;
            UpdatedAt = DateTime.UtcNow;
        }


        /// <summary>
        /// Validates that the rating value is between 1 and 5.
        /// </summary>
        private static int ValidateRatingValue(int value)
        {
            if (value < 0 || value > 5)
            {
                throw new BadRequestException("Rating value must be between 1 and 5.");
            }
            return value;
        }



    }
}
