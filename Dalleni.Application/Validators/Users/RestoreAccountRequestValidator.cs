using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Domin.Helpers;
using FluentValidation;

namespace Dalleni.Application.Validators.Users
{
    public class RestoreAccountRequestValidator : AbstractValidator<RestoreAccountRequest>
    {
        public RestoreAccountRequestValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Must(e => ValidationsAndUniques.IsValid(ValidationsAndUniques.StringType.EMAIL, e))
                .WithMessage("Invalid email format.")
                ;

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(p => ValidationsAndUniques.IsValid(ValidationsAndUniques.StringType.PHONE_NO, p))
                .WithMessage("Invalid phone number format.");
        }
    }
}