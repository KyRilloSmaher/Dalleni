using Dalleni.Domin.Enums;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Votes.Commands.VoteQuestion
{
    public record VoteQuestionCommand(Guid QuestionId, Guid UserId, VoteType Type) : IRequest<Response<bool>>;
}
