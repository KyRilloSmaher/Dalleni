namespace Dalleni.Application.DTOs.Requests.Auth
{
    public class ConfirmResetPasswordCodeRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
