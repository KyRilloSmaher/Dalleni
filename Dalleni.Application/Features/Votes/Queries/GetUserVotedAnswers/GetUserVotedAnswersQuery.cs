using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Votes.Queries.GetUserVotedAnswersQuery
{
    public record GetUserVotedAnswersQuery(Guid userId):IRequest<Response<IEnumerable<VotedAnswerResponse>>>;
}