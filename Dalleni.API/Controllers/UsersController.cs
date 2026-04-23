using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Application.Features.Users.Commands.DeleteAccount;
using Dalleni.Application.Features.Users.Commands.RestoreAccount;
using Dalleni.Application.Features.Users.Commands.UpdateProfile;
using Dalleni.Application.Features.Users.Commands.UpdateRrofileImage;
using Dalleni.Application.Features.Users.Queries.GetUserByEmail;
using Dalleni.Application.Features.Users.Queries.GetUserById;
using Dalleni.Application.Features.Users.Queries.SearchUserByName;
using Dalleni.Application.Features.Users.Queries.GetTopUsers;
using Dalleni.Application.Features.Users.Queries.GetTopContributors;
using Dalleni.Application.Features.Users.Queries.GetUserStats;
using Dalleni.Application.Validators.Users;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Retrieve user details by their unique identifier.
        /// </summary>
        [HttpGet(APIROUTES.User.GetById)]

        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdlQuery(id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Retrieve Cuurent user details .
        /// </summary>

        [HttpGet(APIROUTES.User.GetCurrentUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userIdGuid = Guid.Parse(userId);
            var result = await _mediator.Send(new GetUserByIdlQuery(userIdGuid));
            return FinalResponse(result);
        }

        /// <summary>
        /// Retrieve user details by their Email
        /// </summary>
        [HttpGet(APIROUTES.User.GetByEmail)]

        public async Task<IActionResult> GetUserByEmailAsync([FromRoute]string email)
        {
            var result = await _mediator.Send(new GetUserByEmailQuery(email));
            return FinalResponse(result);
        }

        ///<summary>
        ///Search for users by name or Username.
        ///</summary>
        [HttpGet(APIROUTES.User.Search)]
        public async Task<IActionResult> SearchUsersAsync([FromQuery]string query)
        {
            var result = await _mediator.Send(new SearchUsersByNameQuery(query));
            return FinalResponse(result);
        }


        /// <summary>
        /// Retrieve all users with pagination and optional filtering.
        /// </summary>
        //[HttpGet(APIROUTES.User.GetAll)]
        //public async Task<IActionResult> GetAllUsersAsync([FromQuery] PaginationFilter paginationFilter, [FromQuery] string? search)
        //{
        //    var result = await _mediator.Send(new GetAllUsersQuery(paginationFilter, search));
        //    return FinalResponse(result);
        //}

        /// <summary>
        /// Update the profile information of the currently authenticated user.
        /// </summary>
        [HttpPut(APIROUTES.User.UpdateProfile)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserProfileAsync([FromBody] UpdateUserAccount dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userIdGuid = Guid.Parse(userId);
            var result = await _mediator.Send(new UpdateProfileCommand(dto));
            return FinalResponse(result);
        }
        /// <summary>
        /// Update profile image of the currently authenticated user.
        /// </summary>

        [HttpPut(APIROUTES.User.UpdateProfileImage)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserProfileImageAsync([FromForm] UpdateUserProfileImage dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userIdGuid = Guid.Parse(userId);
            var result = await _mediator.Send(new UpdateProfileImageCommand(dto));
            return FinalResponse(result);
        }
        /// <summary> 
        /// Restore a soft-deleted user account by their unique identifier.
        /// </summary>

        [HttpPost(APIROUTES.User.Restore)]
        [ProducesResponseType(typeof(Response<TokenReponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RestoreUserAsync([FromBody] RestoreAccountRequest request)
        {
            var result = await _mediator.Send(new RestoreAccountCommand(request));
            return FinalResponse(result);
        }
        /// <summary>
        /// Delete a user account by their unique identifier. This is a soft delete, allowing for potential restoration later.
        /// </summary>

        [HttpDelete(APIROUTES.User.Delete)]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            var result = await _mediator.Send(new DeleteAccountCommand(id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get top users by reputation.
        /// </summary>
        [HttpGet(APIROUTES.User.GetTopUsers)]
        [ProducesResponseType(typeof(Response<IEnumerable<UserResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopUsersAsync([FromQuery] int count = 10)
        {
            var result = await _mediator.Send(new GetTopUsersQuery(count));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get top contributors.
        /// </summary>
        [HttpGet(APIROUTES.User.GetTopContributors)]
        [ProducesResponseType(typeof(Response<IEnumerable<UserResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopContributorsAsync([FromQuery] int count = 10)
        {
            var result = await _mediator.Send(new GetTopContributorsQuery(count));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get user statistics.
        /// </summary>
        [HttpGet(APIROUTES.User.GetStats)]
        [ProducesResponseType(typeof(Response<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserStatsAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetUserStatsQuery(id));
            return FinalResponse(result);
        }
    }
}
