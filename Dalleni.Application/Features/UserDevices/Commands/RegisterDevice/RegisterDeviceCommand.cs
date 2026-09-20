using Dalleni.Application.DTOs.Requests.UserDevices;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.UserDevices.Commands.RegisterDevice
{
    public record RegisterDeviceCommand(Guid UserId, RegisterDeviceRequestDto Request): IRequest<Response<bool>>;
}