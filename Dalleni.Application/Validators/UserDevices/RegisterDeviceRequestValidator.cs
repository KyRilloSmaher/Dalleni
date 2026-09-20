using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Application.DTOs.Requests.UserDevices;
using FluentValidation;

namespace Dalleni.Application.Validators.UserDevices
{
    public class RegisterDeviceRequestValidator : AbstractValidator<RegisterDeviceRequestDto>
    {
        public RegisterDeviceRequestValidator()
        {
            RuleFor(x => x.DeviceToken)
                .NotEmpty().WithMessage("Device token is required.")
                .MaximumLength(100).WithMessage("Device token cannot exceed 100 characters.");
            RuleFor(x => x.Platform)
                .NotEmpty().WithMessage("Platform is required.")
                .IsInEnum().WithMessage("Invalid platform value.");
        }
    }
}
