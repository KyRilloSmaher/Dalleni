using Dalleni.Application.Features.Answers.Commands.CreateAnswer;

namespace Dalleni.Application.DTOs.Responses.Votes
{
    public class NewVoteResponse
    {
        public Guid VoteId { get; private set; }
        public int NewUpVotesCount { get; private set; }
        public int NewDownVotesCount { get; private set; }
        public NewVoteResponse(){}

        public  NewVoteResponse(Guid voteId , int newUpvotesCount, int newDownvotesCount)
        {
            VoteId=voteId;
            NewUpVotesCount = newUpvotesCount;
            NewDownVotesCount = newDownvotesCount;
        }

        public static NewVoteResponse Create(Guid voteId , int newUpvotesCount, int newDownvotesCount)
        {
            return new NewVoteResponse(voteId,newUpvotesCount,newDownvotesCount);
        }

    }
    
}