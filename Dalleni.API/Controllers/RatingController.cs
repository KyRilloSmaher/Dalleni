using Dalleni.API.Bases;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Dalleni.Domin.Helpers;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Application.Features.Rating.Queries.GetRatingById;
using Dalleni.Application.Features.Rating.Queries.GetRatingsByServiceIdQuery;
using Dalleni.Application.Features.Rating.Queries.GetRatingByUserId;
using Dalleni.Application.Features.Rating.Queries.GetUserRatingForService;
using Dalleni.Application.Features.Ratings.Commands.CreateRating;
using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.Features.Ratings.Commands.UpdateRating;
using Dalleni.Application.Features.Ratings.Commands.DeleteRating;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RatingsController : BaseController
    {
        public RatingsController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get a rating by ID
        /// </summary>
        [HttpGet(APIROUTES.Ratings.GetById)]
        [ProducesResponseType(typeof(Response<RatingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetRatingByIdQuery(id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get all ratings for a specific service
        /// </summary>
        [HttpGet(APIROUTES.Ratings.GetByServiceId)]
        [ProducesResponseType(typeof(Response<IEnumerable<RatingDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByServiceIdAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetRatingsByServiceIdQuery(id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get all ratings by the current user
        /// </summary>
        [HttpGet(APIROUTES.Ratings.GetMyRatings)]
        [ProducesResponseType(typeof(Response<IEnumerable<RatingDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyRatingsAsync()
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new GetRatingsByUserIdQuery(userId));
            return FinalResponse(result);
        }


        /// <summary>
        /// Get the current user's rating for a specific service
        /// </summary>
        [HttpGet(APIROUTES.Ratings.GetMyRatingForService)]
        [ProducesResponseType(typeof(Response<RatingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyRatingForServiceAsync([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new GetUserRatingForServiceQuery(userId,id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Create a new rating for a service
        /// </summary>
        [HttpPost(APIROUTES.Ratings.Create)]
        [ProducesResponseType(typeof(Response<RatingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRatingRequestDto dto)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new CreateRatingCommand(userId,dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Update an existing rating
        /// </summary>
        [HttpPut(APIROUTES.Ratings.Update)]
        [ProducesResponseType(typeof(Response<RatingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] Guid id,
            [FromBody] UpdateRatingRequestDto dto)
        {
            var userId = GetCurrentUserId();
            dto.RateId = id;
            var result = await _mediator.Send(new UpdateRatingCommand( userId, dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Delete a rating (soft delete)
        /// </summary>
        [HttpDelete(APIROUTES.Ratings.Delete)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new DeleteRatingCommand(id, userId));
            return FinalResponse(result);
        }


    }
}
