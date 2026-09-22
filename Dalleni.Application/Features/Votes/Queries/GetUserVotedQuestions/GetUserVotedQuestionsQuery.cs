using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Votes.Queries.GetUserVotedQuestionsQuery
{
    public record GetUserVotedQuestionsQuery(Guid userId):IRequest<Response<IEnumerable<VotedQuestionResponse>>>;
}