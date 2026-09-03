using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetServicesByCategory
{
    public sealed record GetServicesByCategoryQuery(
        Guid CategoryId
    ) : IRequest<Response<IEnumerable<ServiceDto>>>;
}