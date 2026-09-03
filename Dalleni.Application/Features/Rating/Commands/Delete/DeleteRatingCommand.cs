using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Ratings.Commands.DeleteRating
{
    public record DeleteRatingCommand(Guid rateId, Guid userId) : IRequest<Response<bool>>;
}