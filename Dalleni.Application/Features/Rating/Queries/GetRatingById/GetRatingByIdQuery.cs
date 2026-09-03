
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Rating.Queries.GetRatingById
{
    public record GetRatingByIdQuery(Guid Id) : IRequest<Response<RatingDto>>;
    
}