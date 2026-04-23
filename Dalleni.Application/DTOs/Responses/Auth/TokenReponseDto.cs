using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DTOs.Responses.Auth
{
    public class TokenReponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string TokenType { get; set; } = "Bearer";
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
