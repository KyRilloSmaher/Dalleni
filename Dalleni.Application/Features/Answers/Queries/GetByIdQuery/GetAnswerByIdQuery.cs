
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Domin.ResponsePattern;
using MediatR;


namespace Dalleni.Application.Features.Answers.Queries
{
    public record GetAnswerByIdQuery(Guid Id) : IRequest<Response<AnswerDto>>;
}