namespace Dalleni.Application.DTOs.Requests.Questions
{
    public class UpdateQuestionRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
    }
}
