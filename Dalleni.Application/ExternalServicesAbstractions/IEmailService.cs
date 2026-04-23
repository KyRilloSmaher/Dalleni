namespace Dalleni.Application.ExternalServicesAbstractions
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string? email, string subject, string message);
        Task<bool> SendPasswordResetEmailAsync(string? email, string subject, string otp);
    }
}
