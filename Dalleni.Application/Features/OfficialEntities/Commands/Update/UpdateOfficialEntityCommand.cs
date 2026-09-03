using Dalleni.Application.DTOs.Requests.OfficialEntities;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Commands.UpdateOfficialEntity
{
    public sealed record UpdateOfficialEntityCommand(Guid Id,Guid userId ,UpdateOfficialEntityRequestDto Dto): IRequest<Response<bool>>;
}