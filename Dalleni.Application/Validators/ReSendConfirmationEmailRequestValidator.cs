using Dalleni.Application.DTOs.Requests.Auth;
using FluentValidation;

namespace Dalleni.Application.Validators
{
    public class ReSendConfirmationEmailRequestValidator : AbstractValidator<ReSendConfirmationEmailRequest>
    {
        public ReSendConfirmationEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress();
        }
    }
}
