using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetServicesByOfficialEntity
{
    public sealed record GetServicesByOfficialEntityQuery(
        Guid OfficialEntityId
    ) : IRequest<Response<IEnumerable<ServiceDto>>>;
}