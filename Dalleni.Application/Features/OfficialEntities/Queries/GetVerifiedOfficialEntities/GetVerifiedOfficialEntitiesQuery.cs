using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.GetVerifiedOfficialEntities
{
    public sealed record GetVerifiedOfficialEntitiesQuery(PagedRequest request)
        : IRequest<Response<PaginatedResult<OfficialEntityDto>>>;
}