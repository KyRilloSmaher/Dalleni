using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Commands.RestoreOfficialEntity
{
    public sealed record RestoreOfficialEntityCommand(Guid Id ,Guid userId): IRequest<Response<bool>>;
}