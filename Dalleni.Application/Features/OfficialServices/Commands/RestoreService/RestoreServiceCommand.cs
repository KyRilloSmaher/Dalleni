using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.RestoreService
{
    public sealed record RestoreServiceCommand(
        Guid Id,
        Guid UserId
    ) : IRequest<Response<bool>>;
}