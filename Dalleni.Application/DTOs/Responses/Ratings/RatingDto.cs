
namespace Dalleni.Application.DTOs.Responses.Ratings
{
    public class RatingDto
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public Guid UserId { get; set; }
        public int Value { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? UserName { get;  set; }
    }
}