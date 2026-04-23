using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Queries.GetPagedQuestions
{
    public record GetPagedQuestionsQuery(PagedRequest request) : IRequest<Response<PaginatedResult<QuestionSummaryDto>>>;
}
