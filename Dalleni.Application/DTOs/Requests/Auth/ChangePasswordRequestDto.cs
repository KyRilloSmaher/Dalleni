namespace Dalleni.Application.DTOs.Requests.Auth
{
    public class ChangePasswordRequestDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
