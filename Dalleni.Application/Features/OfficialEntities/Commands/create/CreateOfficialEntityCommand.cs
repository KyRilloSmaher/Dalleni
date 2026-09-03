using Dalleni.Application.DTOs.Requests.OfficialEntities;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Create
{
    public sealed record CreateOfficialEntityCommand(
        Guid currentUserId,
        CreateOfficialEntityRequestDto dto
    ) : IRequest<Response<Guid>>;
}