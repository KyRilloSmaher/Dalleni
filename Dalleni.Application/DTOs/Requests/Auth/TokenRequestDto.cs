using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DTOs.Requests.Auth
{
    public class TokenRequestDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
