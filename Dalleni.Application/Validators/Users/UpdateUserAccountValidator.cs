using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Domin.Helpers;
using FluentValidation;

namespace Dalleni.Application.Validators.Users
{
    public class UpdateUserAccountValidator : AbstractValidator<UpdateUserAccount>
    {
        public UpdateUserAccountValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User Id is required.");

            RuleFor(x => x.FirstName)
              .NotEmpty().WithMessage("First name is required.")
              .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Must(u => ValidationsAndUniques.IsValid(ValidationsAndUniques.StringType.USERNAME, u))
                .WithMessage("Username must be 3–20 characters (letters, digits, underscores, or dots).")
              ;

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(p => ValidationsAndUniques.IsValid(ValidationsAndUniques.StringType.PHONE_NO, p))
                .WithMessage("Invalid phone number format.");
        }
    }
}