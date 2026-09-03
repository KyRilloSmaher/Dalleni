
using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Members.CreateOwner
{
    public record CreateEntityOwnerCommand(
        SignUpRequest SignUpRequestDto,
        Guid entityId
    ) : IRequest<Response<bool>>;
    
}