using Dalleni.Application.DTOs.Requests.Services;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.CreateService
{
    public sealed record CreateServiceCommand(CreateServiceRequestDto Dto,Guid UserId) : IRequest<Response<ServiceDto>>;
}