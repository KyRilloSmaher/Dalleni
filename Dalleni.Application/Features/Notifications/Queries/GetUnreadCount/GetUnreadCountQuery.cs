using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Notifications.Queries.GetUnreadCount
{
    /// <summary>
    /// Retrieves the number of unread notifications for the authenticated user.
    /// </summary>
    public record GetUnreadCountQuery(Guid UserId) : IRequest<Response<int>>;
}