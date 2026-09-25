using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.UserDevices;
using Dalleni.Application.Features.UserDevices.Commands.DeactivateDevice;
using Dalleni.Application.Features.UserDevices.Commands.RegisterDevice;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserDevicesController : BaseController
    {
        public UserDevicesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Registers a new device for the authenticated user.
        /// </summary>
        /// <param name="request">The device registration request data.</param>
        /// <returns></returns>
        [HttpPost(APIROUTES.UserDevices.RegisterDevice)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterDeviceAsync([FromBody] RegisterDeviceRequestDto request)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new RegisterDeviceCommand(userId, request));
            return FinalResponse(result);   
        }

        /// <summary>
        /// Deactivates a device for the authenticated user.
        /// </summary>
        /// <param name="deviceId">The ID of the device to deactivate.</param>
        /// <returns></returns>
        [HttpPut(APIROUTES.UserDevices.DeactivateDevice)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeactivateDeviceAsync([FromQuery] Guid deviceId)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new DeactivateDeviceCommand(userId, deviceId));
            return FinalResponse(result);
        }
    }
}