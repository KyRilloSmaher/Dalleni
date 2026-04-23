namespace Dalleni.Application.DTOs.Responses.Tags
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QuestionCount { get; set; }
    }
}
