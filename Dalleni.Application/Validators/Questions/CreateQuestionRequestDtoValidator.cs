using Dalleni.Application.DTOs.Requests.Questions;
using FluentValidation;

namespace Dalleni.Application.Validators.Questions
{
    public class CreateQuestionRequestDtoValidator : AbstractValidator<CreateQuestionRequestDto>
    {
        public CreateQuestionRequestDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(20).WithMessage("Content must be at least 20 characters.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required.");

            RuleFor(x => x.Tags)
                .Must(t => t == null || t.Count <= 5).WithMessage("Maximum 5 tags allowed.");
        }
    }
}
