using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Requests.OfficialEntities;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Commands.DeleteOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Commands.RestoreOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Commands.UpdateOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Commands.VerifyOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Create;
using Dalleni.Application.Features.OfficialEntities.Members.AcceptInvitation;
using Dalleni.Application.Features.OfficialEntities.Members.Invite;
using Dalleni.Application.Features.OfficialEntities.Queries.GetAllOfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Queries.GetMyOfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Queries.GetOfficialEntityById;
using Dalleni.Application.Features.OfficialEntities.Queries.GetVerifiedOfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Queries.SearchOfficialEntities;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OfficialEntitiesController : BaseController
    {
        public OfficialEntitiesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get official entity by ID
        /// </summary>
        [HttpGet(APIROUTES.OfficialEntities.GetById)]
        [ProducesResponseType(typeof(Response<OfficialEntityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetOfficialEntityByIdQuery(id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get all official entities (paginated)
        /// </summary>
        [HttpGet(APIROUTES.OfficialEntities.GetAll)]
        [ProducesResponseType(typeof(Response<PaginatedResult<OfficialEntityDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync([FromQuery]PagedRequest request)
        {
            var result = await _mediator.Send(new GetAllOfficialEntitiesQuery(request));
            return FinalResponse(result);
        }


        /// <summary>
        /// Get verified official entities
        /// </summary>
        [HttpGet(APIROUTES.OfficialEntities.GetVerifiedEntities)]
        [ProducesResponseType(typeof(Response<IEnumerable<OfficialEntityDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVerifiedAsync([FromQuery]PagedRequest request)
        {
            var result = await _mediator.Send(new GetVerifiedOfficialEntitiesQuery(request));
            return FinalResponse(result);
        }

        /// <summary>
        /// Search official entities by name or description
        /// </summary>
        [HttpGet(APIROUTES.OfficialEntities.Search)]
        [ProducesResponseType(typeof(Response<IEnumerable<OfficialEntityDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAsync([FromQuery] string keyword)
        {
            var result = await _mediator.Send(new SearchOfficialEntitiesQuery(keyword));
            return FinalResponse(result);
        }
        /// <summary>
        /// Get official entity where the current user is a member.
        /// </summary>
        [HttpGet(APIROUTES.OfficialEntities.GetMyEntities)]
        [ProducesResponseType(typeof(Response<IEnumerable<OfficialEntityDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyEntitiesAsync()
        {
            var userId = GetCurrentUserId();
            var result =await _mediator.Send(new GetMyOfficialEntityQuery(userId));
            return FinalResponse(result);
        }



        /// <summary>
        /// Create a new official entity
        /// </summary>
        [HttpPost(APIROUTES.OfficialEntities.Create)]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromForm] CreateOfficialEntityRequestDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new CreateOfficialEntityCommand(userId ,dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Update official entity details
        /// </summary>
        [HttpPut(APIROUTES.OfficialEntities.Update)]
        // [Authorize(Roles = "Admin,EntityMember")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,[FromForm] UpdateOfficialEntityRequestDto dto)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new UpdateOfficialEntityCommand(id,  userId,dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Verify an official entity (Admin only)
        /// </summary>
        [HttpPost(APIROUTES.OfficialEntities.Verify)]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyAsync([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new VerifyOfficialEntityCommand(id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Delete official entity (Soft delete)
        /// </summary>
        [HttpDelete(APIROUTES.OfficialEntities.Delete)]
        [Authorize(Roles = "Admin,EntityMember")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new DeleteOfficialEntityCommand(id, userId));
            return FinalResponse(result);
        }

        /// <summary>
        /// Restore a deleted official entity
        /// </summary>
        [HttpPost(APIROUTES.OfficialEntities.Restore)]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreAsync([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new RestoreOfficialEntityCommand(id,userId));
            return FinalResponse(result);
        }


    }
}