using Dalleni.Application.DTOs.Requests.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Commands.UpdateQuestion
{
    public record UpdateQuestionCommand(Guid Id, UpdateQuestionRequestDto dto, Guid UserId) : IRequest<Response<bool>>;
}
