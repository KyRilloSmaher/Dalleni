using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Users.Queries.GetTopContributors
{
    public record GetTopContributorsQuery(int Count) : IRequest<Response<IEnumerable<UserResponseDto>>>;
}
