
using Microsoft.AspNetCore.Http;

namespace Dalleni.Application.DTOs.Requests.OfficialEntities
{

    public class CreateOfficialEntityRequestDto
    {
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public IFormFile? Logo { get; set; }
        
        public string? WebsiteUrl { get; set; }
    }
}