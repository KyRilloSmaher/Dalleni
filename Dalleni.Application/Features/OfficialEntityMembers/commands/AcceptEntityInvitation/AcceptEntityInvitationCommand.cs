using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Members.AcceptInvitation
{
    public sealed record AcceptEntityInvitationCommand(
        Guid currentUserId,
        string Token
    ) : IRequest<Response<bool>>;
}