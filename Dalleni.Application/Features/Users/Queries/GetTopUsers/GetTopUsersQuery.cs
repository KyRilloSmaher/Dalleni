using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Users.Queries.GetTopUsers
{
    public record GetTopUsersQuery(int Count) : IRequest<Response<IEnumerable<UserResponseDto>>>;
}
