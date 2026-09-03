
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Rating.Queries.GetRatingByUserId
{
    public record GetRatingsByUserIdQuery(Guid UserId) : IRequest<Response<IEnumerable<RatingDto>>>;
    
}