using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Users.Queries.GetUserStats
{
    public record GetUserStatsQuery(Guid UserId) : IRequest<Response<UserResponseDto>>;
}
