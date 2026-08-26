
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Commands.AcceptAnswer
{
    public record AcceptAnswerCommand (Guid id, Guid userId): IRequest<Response<bool>>;

}