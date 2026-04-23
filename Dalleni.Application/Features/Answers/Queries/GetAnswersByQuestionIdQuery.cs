using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Queries
{
    public record GetAnswersByQuestionIdQuery(Guid QuestionId) : IRequest<Response<IEnumerable<AnswerDto>>>;
}
