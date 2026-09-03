using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetServiceById
{
    public sealed record GetServiceByIdQuery(
        Guid Id
    ) : IRequest<Response<ServiceDto>>;
}