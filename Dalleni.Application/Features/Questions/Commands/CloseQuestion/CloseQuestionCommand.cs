using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Commands.CloseQuestion
{
    public record CloseQuestionCommand(Guid Id, Guid UserId) : IRequest<Response<bool>>;
}
