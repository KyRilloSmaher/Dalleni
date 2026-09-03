using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.GetMyOfficialEntities
{
    public sealed record GetMyOfficialEntityQuery(Guid userId): IRequest<Response<OfficialEntityDto>>;
}