using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.SearchServices
{
    public sealed record SearchServicesQuery(
        string Keyword
    ) : IRequest<Response<IEnumerable<ServiceDto>>>;
}