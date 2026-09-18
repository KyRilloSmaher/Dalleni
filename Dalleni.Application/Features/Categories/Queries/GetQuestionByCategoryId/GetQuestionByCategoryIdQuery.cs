using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Categories.Queries.GetQuestionsByCategoryId
{
    public record GetQuestionsByCategoryIdQuery(PagedRequest request, Guid categoryId) : IRequest<Response<PaginatedResult<QuestionSummaryDto>>>;
}
