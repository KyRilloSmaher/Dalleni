using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.ToggleServiceAvailability
{
    public sealed record ToggleServiceAvailabilityCommand(
        Guid Id,
        Guid UserId
    ) : IRequest<Response<bool>>;
}