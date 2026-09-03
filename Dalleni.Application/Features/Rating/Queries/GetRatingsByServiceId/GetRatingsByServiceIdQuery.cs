using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Rating.Queries.GetRatingsByServiceIdQuery
{
    public record GetRatingsByServiceIdQuery(Guid serviceId): IRequest<Response<IEnumerable<RatingDto>>>;
}