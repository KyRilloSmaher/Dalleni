
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Commands.UnAcceptAnswer
{
    public record UnAcceptAnswerCommand (Guid id , Guid userId): IRequest<Response<bool>>;

}