using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Queries.GetQuestionDetails
{
    public record GetQuestionDetailsQuery(Guid Id) : IRequest<Response<QuestionDetailsResponseDto>>;
}
