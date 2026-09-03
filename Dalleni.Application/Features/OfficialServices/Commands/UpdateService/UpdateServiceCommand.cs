using Dalleni.Application.DTOs.Requests.Services;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.UpdateService
{
    public sealed record UpdateServiceCommand(UpdateServiceRequestDto Dto,Guid UserId
    ) : IRequest<Response<ServiceDto>>;
}