using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Commands.DeleteQuestion
{
    public record DeleteQuestionCommand(Guid Id, Guid UserId) : IRequest<Response<bool>>;
}
