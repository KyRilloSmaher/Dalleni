
using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Notifications;
using Dalleni.Application.Features.Notifications.Commands.MarkAsRead;
using Dalleni.Application.Features.Notifications.Queries.GetMyNotifications;
using Dalleni.Application.Features.Notifications.Queries.GetUnreadCount;
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
    public class NotificationsController : BaseController
    {
        public NotificationsController(IMediator mediator) : base(mediator)
        {
        }

         /// <summary>
         /// Retrieves the notifications for the authenticated user with pagination support.
         /// </summary>
         /// <param name="PagedRequest"> The pagination request parameters. </param>
         /// <returns></returns>
        [HttpGet(APIROUTES.Notifications.GetMyNotifications)]
        [ProducesResponseType(typeof(Response<IEnumerable<NotificationResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyNotificationsAsync([FromQuery] PagedRequest PagedRequest)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new GetMyNotificationsQuery(userId, PagedRequest));
            return FinalResponse(result);
        }
        /// <summary>
        /// Retrieves the number of unread notifications for the authenticated user.
        /// </summary>
        /// <returns></returns>
        [HttpGet(APIROUTES.Notifications.GetUnreadCount)]
        [ProducesResponseType(typeof(Response<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUnreadCountAsync()
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new GetUnreadCountQuery(userId));
            return FinalResponse(result);
        }
        /// <summary>
        /// Marks a specific notification as read for the authenticated user.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(APIROUTES.Notifications.MarkAsRead)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MarkAsReadAsync(Guid id)
        {
             var userId = GetCurrentUserId();
            var result = await _mediator.Send(new MarkNotificationAsReadCommand(userId,id));
            return FinalResponse(result);
        }
    }
}