using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetAllServices
{
    public sealed record GetAllServicesQuery(PagedRequest request)
    : IRequest<Response<PaginatedResult<ServiceDto>>>;
}