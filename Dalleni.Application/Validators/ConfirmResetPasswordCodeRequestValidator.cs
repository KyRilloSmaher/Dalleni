using Dalleni.Application.DTOs.Requests.Auth;
using FluentValidation;

namespace Dalleni.Application.Validators
{
    public class ConfirmResetPasswordCodeRequestValidator : AbstractValidator<ConfirmResetPasswordCodeRequest>
    {
        public ConfirmResetPasswordCodeRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress();

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .Length(6).WithMessage("Code must be 6 characters.");
        }
    }
}
