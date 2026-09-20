using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.UserDevices.Commands.DeactivateDevice
{
    public record DeactivateDeviceCommand (Guid UserId , Guid DeviceId): IRequest<Response<bool>>;
    
}