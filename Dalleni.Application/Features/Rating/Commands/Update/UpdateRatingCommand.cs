using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Ratings.Commands.UpdateRating
{
    public record UpdateRatingCommand(Guid userId , UpdateRatingRequestDto Dto) : IRequest<Response<RatingDto>>;
}