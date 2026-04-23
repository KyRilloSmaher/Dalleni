using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DTOs.Requests.Users
{
    public class RestoreAccountRequest
    {
       public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
