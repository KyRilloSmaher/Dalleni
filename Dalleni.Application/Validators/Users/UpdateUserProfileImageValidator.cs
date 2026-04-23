using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Domin.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Dalleni.Application.Validators.Users
{
    public class UpdateUserProfileImageValidator : AbstractValidator<UpdateUserProfileImage>
    {
        public UpdateUserProfileImageValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User Id is required.");

            RuleFor(x => x.ProfileImage)
                .NotNull().WithMessage("User image is required.")
                .Must(ValidationsAndUniques.BeAValidImage)
                .WithMessage("Image must be jpg, jpeg, or png, and not exceed 5MB.");
        }
    }
}