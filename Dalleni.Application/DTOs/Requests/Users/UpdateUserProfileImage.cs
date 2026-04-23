using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.DTOs.Requests.Users
{
    public class UpdateUserProfileImage
    {
        public Guid Id { get; set; }
        public IFormFile ProfileImage { get; set; } 
    }
}
