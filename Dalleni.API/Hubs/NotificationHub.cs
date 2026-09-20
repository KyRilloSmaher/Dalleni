using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Dalleni.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("On Connect .....");

            var userIdClaim =
                Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? Context.User?.FindFirst("sub")?.Value;

            Console.WriteLine($"User ID Claim: {userIdClaim}");
            Console.WriteLine($"Is Authenticated: {Context.User?.Identity?.IsAuthenticated}");

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException(
                    "User ID not found or invalid in claims.");
            }

            var groupName = GetUserGroup(userId);

            Console.WriteLine($"Adding connection {Context.ConnectionId} to group {groupName}");

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                groupName);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            var userIdClaim =
                Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? Context.User?.FindFirst("sub")?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                await Groups.RemoveFromGroupAsync(
                    Context.ConnectionId,
                    GetUserGroup(userId));
            }

            await base.OnDisconnectedAsync(exception);
        }

        private static string GetUserGroup(Guid userId)
        {
            return $"user:{userId}";
        }
    }
}