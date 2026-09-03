
namespace Dalleni.Application.DTOs.Responses.Services
{
  public class ServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RequiredDocuments { get; set; } = string.Empty;
    public decimal? Fees { get; set; }
    public string? Category { get; set; }
    public Guid CategoryId {get;set;}
    public bool IsAvailable { get; set; }
    public Guid OfficialEntityId { get; set; }
    public string OfficialEntityName { get; set; } = string.Empty;
    public bool IsOfficialEntityVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public double AverageRating { get; set; }
}
}