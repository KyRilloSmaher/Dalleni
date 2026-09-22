using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.Features.Answers.Commands.CreateAnswer;

namespace Dalleni.Application.DTOs.Responses.Votes
{
    public class VotedQuestionResponse
    {
        public Guid VoteId { get; set;}
        public QuestionSummaryDto? Question {get;set;}
        public DateTime CreatedAt {get;set;}
    }
    
}