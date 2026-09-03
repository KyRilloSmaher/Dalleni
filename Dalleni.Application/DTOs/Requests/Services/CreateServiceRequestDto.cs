
namespace Dalleni.Application.DTOs.Requests.Services
{
    public class CreateServiceRequestDto
    {

        public Guid OfficialEntityId { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;

        public string RequiredDocuments { get; set; } = string.Empty;
        
        public decimal? Fees { get; set; }

        public Guid CategoryId { get; set; }
        public bool IsAvailable { get; set; }
        
    }
}