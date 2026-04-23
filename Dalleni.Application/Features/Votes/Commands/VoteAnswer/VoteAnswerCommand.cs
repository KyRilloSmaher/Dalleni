using Dalleni.Domin.Enums;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Votes.Commands.VoteAnswer
{
    public record VoteAnswerCommand(Guid AnswerId, Guid UserId, VoteType Type) : IRequest<Response<bool>>;
}
