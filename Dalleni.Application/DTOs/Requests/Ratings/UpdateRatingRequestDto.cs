namespace Dalleni.Application.DTOs.Requests.Ratings
{
    public class UpdateRatingRequestDto
    {
        public Guid RateId {get;set;}
        public int Value {get;set;}
        public string? Comment {get;set;}
    }
}