namespace Dalleni.Application.DTOs.Requests.Auth
{
    public class SetNewPasswordRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
