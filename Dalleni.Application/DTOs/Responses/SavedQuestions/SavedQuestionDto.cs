using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.DTOs.Responses.Users;

namespace Dalleni.Application.DTOs.Responses.SavedQuestions
{
    public class SavedQuestionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
        public DateTime SavedAt { get; set; }
        public QuestionDetailsResponseDto? Question { get; set; }
    }
}