using Dalleni.Application.DTOs.Requests.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Commands.CreateQuestion
{
    public record CreateQuestionCommand(CreateQuestionRequestDto dto, Guid UserId) : IRequest<Response<Guid>>;
}
