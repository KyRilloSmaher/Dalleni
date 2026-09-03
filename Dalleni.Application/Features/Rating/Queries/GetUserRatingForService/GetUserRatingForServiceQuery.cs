using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Rating.Queries.GetUserRatingForService
{
    public record GetUserRatingForServiceQuery (Guid userId , Guid serviceId) : IRequest<Response<RatingDto>>;
}