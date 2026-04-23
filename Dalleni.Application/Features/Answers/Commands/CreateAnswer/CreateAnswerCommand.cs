using Dalleni.Application.DTOs.Requests.Answers;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Commands.CreateAnswer
{
    public record CreateAnswerCommand(CreateAnswerRequestDto dto, Guid UserId) : IRequest<Response<Guid>>;
}
