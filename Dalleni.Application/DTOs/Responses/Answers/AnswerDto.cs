namespace Dalleni.Application.DTOs.Responses.Answers
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorProfileImageUrl { get; set; }
        public int AuthorReputation { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
