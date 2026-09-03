using Dalleni.Application.DTOs.Requests.Ratings;

using FluentValidation;

namespace Dalleni.Application.Validators.Ratings
{
    public class CreateRatingRequestDtoValidator : AbstractValidator<CreateRatingRequestDto>
    {
        public CreateRatingRequestDtoValidator()
        {
            RuleFor(x => x.ServiceId)
                .NotEmpty().WithMessage("Service ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Service ID must be a valid GUID.");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Rating value is required.")
                .InclusiveBetween(1, 5).WithMessage("Rating value must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Comment));

            RuleFor(x => x.UserName)
                .MaximumLength(100).WithMessage("User name must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            // Optional: If you want to validate that Comment is required when Value is low
            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Please provide a comment explaining your rating.")
                .When(x => x.Value <= 2)
                .WithMessage("For ratings of 2 stars or less, a comment is required.");
        }
    }
}