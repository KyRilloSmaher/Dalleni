using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Commands.DeleteOfficialEntity
{
    public sealed record DeleteOfficialEntityCommand(Guid Id , Guid UserId): IRequest<Response<bool>>;
}