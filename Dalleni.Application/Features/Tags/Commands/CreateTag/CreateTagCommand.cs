using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Tags.Commands.CreateTag
{
    public record CreateTagCommand(string Name, string? Description) : IRequest<Response<Guid>>;
}
