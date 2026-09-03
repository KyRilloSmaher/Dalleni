using System.Security.Cryptography;
using System.Text;
using Dalleni.Domin.Interfaces.Services;

namespace Dalleni.Application.Services
{
    public class InvitationTokenGeneratorService : IInvitationTokenGeneratorService
    {
        public  string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public  string HashToken(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);

            var hash = SHA256.HashData(bytes);

            return Convert.ToHexString(hash);
        }
    }
}