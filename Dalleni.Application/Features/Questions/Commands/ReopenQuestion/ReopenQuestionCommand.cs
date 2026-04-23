using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Commands.ReopenQuestion
{
    public record ReopenQuestionCommand(Guid Id, Guid UserId) : IRequest<Response<bool>>;
}
