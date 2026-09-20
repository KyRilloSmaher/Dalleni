using Dalleni.Domin.Enums;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Votes.Commands.DeleteVote
{
    public record DeleteVoteCommand(Guid UserId , Guid voteId ) : IRequest<Response<bool>>;
}
