using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Commands.AcceptAnswer
{
    public record AcceptAnswerCommand(Guid QuestionId, Guid AnswerId, Guid UserId) : IRequest<Response<bool>>;
}
