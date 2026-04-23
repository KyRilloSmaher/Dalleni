using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Application.DTOs.Responses.Tags;

namespace Dalleni.Application.DTOs.Responses.Questions
{
    public class QuestionDetailsResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorProfileImageUrl { get; set; }
        public int AuthorReputation { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int Views { get; set; }
        public int AnswersCount { get; set; }
        public bool IsClosed { get; set; }
        public Guid? AcceptedAnswerId { get; set; }
        public double Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<TagDto> Tags { get; set; } = new();
        public List<AnswerDto> Answers { get; set; } = new();
    }

    public class QuestionSummaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int Views { get; set; }
        public int AnswersCount { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TagDto> Tags { get; set; } = new();
    }
}
