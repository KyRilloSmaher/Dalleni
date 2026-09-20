using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Dalleni.Application.DTOs.Responses.Notifications;
using Dalleni.Application.DTOs.Requests.Base;

namespace Dalleni.Application.Features.Notifications.Queries.GetMyNotifications
{
    /// <summary>
    /// Retrieves the authenticated user's notifications.
    /// </summary>
    public record GetMyNotificationsQuery(Guid UserId, PagedRequest PagedRequest) : IRequest<Response<PaginatedResult<NotificationResponseDto>>>
    {
    }
}