using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Dalleni.API.Bases
{
    [ApiController]
    public class BaseController: ControllerBase
    {
        protected readonly IMediator _mediator;
        public BaseController (IMediator mediator)
        {
            _mediator = mediator;
        }

        public static ObjectResult FinalResponse<T>(Response<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case HttpStatusCode.Created:
                    return new CreatedResult(string.Empty, response);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case HttpStatusCode.Forbidden:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status403Forbidden };
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(response);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.Accepted:
                    return new AcceptedResult(string.Empty, response);
                case HttpStatusCode.UnprocessableEntity:
                    return new UnprocessableEntityObjectResult(response);
                default:
                    return new BadRequestObjectResult(response);
            }
        }

        /// <summary>
        /// Get the current user's ID from the claims
        /// </summary>
        protected Guid GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                              ?? User?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in claims");
            }

            return userId;
        }
    }
}
