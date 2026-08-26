
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Queries.GetAcceptedAnswersByQuestionId
{
    public record GetAcceptedAnswersByQuestionIdQuery(Guid questionId) : IRequest<Response<IEnumerable<AnswerDto>>>;
}