using Dalleni.Application.DTOs.Requests.Answers;
using FluentValidation;

namespace Dalleni.Application.Validators.Answers
{
    public class CreateAnswerRequestDtoValidator : AbstractValidator<CreateAnswerRequestDto>
    {
        public CreateAnswerRequestDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(10).WithMessage("Content must be at least 10 characters.");

            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("Question identification is required.");
        }
    }
}
