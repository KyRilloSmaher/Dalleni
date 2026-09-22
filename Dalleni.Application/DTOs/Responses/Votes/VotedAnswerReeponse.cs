using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.Features.Answers.Commands.CreateAnswer;

namespace Dalleni.Application.DTOs.Responses.Votes
{
    public class VotedAnswerResponse
    {
        public Guid VoteId { get; set;}
        public AnswerDto? Answer {get;set;}
        public DateTime CreatedAt {get;set;}
       
    }
    
}