using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.DTOs.Requests.Services;
using FluentValidation;

namespace Dalleni.Application.Validators.Ratings
{
    public class UpdateRatingRequestDtoValidator : AbstractValidator<UpdateRatingRequestDto>
    {
        public UpdateRatingRequestDtoValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Rating value is required.")
                .InclusiveBetween(1, 5).WithMessage("Rating value must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Comment));

            // Optional: If you want to validate that Comment is required when Value is low
            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Please provide a comment explaining your rating.")
                .When(x => x.Value <= 2)
                .WithMessage("For ratings of 2 stars or less, a comment is required.");

            // Optional: If you want to validate that at least one field is being updated
            RuleFor(x => x)
                .Must(x => x.Value != 0 || !string.IsNullOrEmpty(x.Comment))
                .WithMessage("At least one field (Value or Comment) must be provided for update.");
        }
    }
}