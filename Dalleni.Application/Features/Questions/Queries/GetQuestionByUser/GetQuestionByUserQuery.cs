
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Queries.GetByUser
{
    public record GetQuestionsByUserQuery(Guid userId):IRequest<Response<IEnumerable<QuestionSummaryDto>>>;
}