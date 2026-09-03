using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.SearchOfficialEntities
{
    public sealed record SearchOfficialEntitiesQuery(
        string Keyword)
        : IRequest<Response<IEnumerable<OfficialEntityDto>>>;
}