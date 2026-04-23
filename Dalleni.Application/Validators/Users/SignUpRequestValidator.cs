using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Domin.Helpers;
using FluentValidation;

namespace Dalleni.Application.Validators.Users
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
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

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Must(e => ValidationsAndUniques.IsValid(ValidationsAndUniques.StringType.EMAIL, e))
                .WithMessage("Invalid email format.")
                ;

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(p => ValidationsAndUniques.IsValid(ValidationsAndUniques.StringType.PHONE_NO, p))
                .WithMessage("Invalid phone number format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Must(p => ValidationsAndUniques.IsValid(ValidationsAndUniques.StringType.PASSWORD, p))
                .WithMessage("Password must contain upper/lowercase letters, digits, symbols, and be at least 12 characters long.");

            RuleFor(x => x.ProfileImage)
                .NotNull().WithMessage("User image is required.")
                .Must(ValidationsAndUniques.BeAValidImage)
                .WithMessage("Image must be jpg, jpeg, or png, and not exceed 5MB.")
                .When(x=>x.ProfileImage is not null);
        }
    }
}
