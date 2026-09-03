
namespace Dalleni.Domin.Interfaces.Services
{
    
    public interface IInvitationTokenGeneratorService
    {
        public string GenerateToken();

        public  string HashToken(string token);
    }
}