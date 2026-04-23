namespace Dalleni.Application.DTOs.Responses.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QuestionCount { get; set; }
    }
}
