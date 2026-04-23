using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DTOs.Requests.Users
{
    public class UpdateUserAccount
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
