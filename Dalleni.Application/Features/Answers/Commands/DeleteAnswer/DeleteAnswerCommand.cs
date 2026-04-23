using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Commands.DeleteAnswer
{
    public record DeleteAnswerCommand(Guid Id, Guid UserId) : IRequest<Response<bool>>;
}
