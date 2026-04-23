namespace Dalleni.Application.DTOs.Responses.Users
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int Reputation { get; set; }
        public int AnswersCount { get; set; }
        public int VerifiedAnswersCount { get; set; }
        public int QuestionsCount { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
