using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Users.Commands.AdjustReputation
{
    public record AdjustReputationCommand(Guid UserId, int Delta) : IRequest<Response<bool>>;
}
