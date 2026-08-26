using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Queries
{
    public record GetAnswersByUserIdQuery(Guid UserId) : IRequest<Response<IEnumerable<AnswerDto>>>;
}