using Dalleni.Application.DTOs.Requests.Answers;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Commands.UpdateAnswer
{
    public record UpdateAnswerCommand(Guid Id, UpdateAnswerRequestDto dto, Guid UserId) : IRequest<Response<bool>>;
}
