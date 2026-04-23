using Dalleni.Application.DTOs.Requests.Auth;
using FluentValidation;

namespace Dalleni.Application.Validators
{
    public class ForgetPasswordRequestDtoValidator : AbstractValidator<ForgetPasswordRequestDto>
    {
        public ForgetPasswordRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress();
        }
    }
}
