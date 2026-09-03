




using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Application.DTOs.Requests.OfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Members.AcceptInvitation;
using Dalleni.Application.Features.OfficialEntities.Members.CreateOwner;
using Dalleni.Application.Features.OfficialEntities.Members.Invite;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OfficialEntityMembers : BaseController
    {
        public OfficialEntityMembers(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Creates an owner for an official entity.
        /// </summary>
        [HttpPost(APIROUTES.OfficialEntityMembers.CreateOwner)]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateEntityOwnerAsync([FromRoute] Guid id, [FromBody] SignUpRequest dto)
        {
            var result = await _mediator.Send(new CreateEntityOwnerCommand(dto, id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Invites a user to become a member of an official entity.
        /// </summary>
        [HttpPost(APIROUTES.OfficialEntityMembers.inviteMember)]
        [Authorize(Roles = "EntitiyADMIN")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InviteMemberAsync([FromRoute] Guid id,[FromBody] InviteEntityMemberRequestDto dto)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new InviteEntityMemberCommand(userId ,id,dto.Email,dto.Role));
            return FinalResponse(result);
        }

        //// <summary>
        /// Accepts an official entity invitation for the authenticated user.
        /// </summary>
        [HttpGet( APIROUTES.OfficialEntityMembers.AcceptInvitation,Name = "AcceptOfficialEntityInvitation")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AcceptInvitationAsync([FromQuery] AcceptEntityInvitationRequestDto dto,CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var result = await _mediator.Send(
                new AcceptEntityInvitationCommand(
                    userId,
                    dto.Token),
                cancellationToken);

            return FinalResponse(result);
        }
    }
}