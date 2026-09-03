using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Ratings.Commands.CreateRating
{
    public record CreateRatingCommand (Guid UserId ,CreateRatingRequestDto Dto): IRequest<Response<RatingDto>>;
    
}