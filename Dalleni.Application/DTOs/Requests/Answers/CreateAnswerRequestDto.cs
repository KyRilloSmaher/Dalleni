namespace Dalleni.Application.DTOs.Requests.Answers
{
    public class CreateAnswerRequestDto
    {
        public Guid QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
