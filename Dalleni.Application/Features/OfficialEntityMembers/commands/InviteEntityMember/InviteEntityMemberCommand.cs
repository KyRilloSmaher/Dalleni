using Dalleni.Domin.Enums;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Members.Invite
{
    public sealed record InviteEntityMemberCommand(
        Guid currentuserId ,
        Guid OfficialEntityId,
        string Email,
        EntityRole Role
    ) : IRequest<Response<bool>>;
}