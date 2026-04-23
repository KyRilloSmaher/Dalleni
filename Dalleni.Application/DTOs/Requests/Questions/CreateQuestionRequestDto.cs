namespace Dalleni.Application.DTOs.Requests.Questions
{
    public class CreateQuestionRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
