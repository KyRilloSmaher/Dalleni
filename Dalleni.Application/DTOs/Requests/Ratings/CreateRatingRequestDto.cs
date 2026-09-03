
namespace Dalleni.Application.DTOs.Requests.Ratings
{
    public class CreateRatingRequestDto 
    {
            public Guid ServiceId { get; set; }

            public int Value { get; set; }

            public string? Comment { get; set; }

            public string? UserName { get; set; }
    }
}