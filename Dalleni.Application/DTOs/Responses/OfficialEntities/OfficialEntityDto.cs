
namespace Dalleni.Application.DTOs.Responses.OfficialEntities
{
    public class OfficialEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public bool IsVerified { get; set; }
        public int ServicesCount { get; set; }

    }
}