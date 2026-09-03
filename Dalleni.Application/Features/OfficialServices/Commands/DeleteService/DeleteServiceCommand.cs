using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.DeleteService
{
    public sealed record DeleteServiceCommand(
        Guid Id,
        Guid UserId
    ) : IRequest<Response<bool>>;
}