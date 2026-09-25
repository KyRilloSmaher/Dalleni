using Dalleni.Domin.Enums;
using Microsoft.AspNetCore.Mvc;
using Dalleni.API.Bases;
namespace Dalleni.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class LookupsController : BaseController
{
    /// <summary>
    /// Gets all available vote types.
    /// </summary>
    [HttpGet("vote-types")]
    public IActionResult GetVoteTypes()
    {
        var list = Enum.GetValues<VoteType>()
            .Select(e => new
            {
                Value = (int)e,
                DisplayName = e.ToString()
            })
            .ToList();

        return Ok(list);
    }

    /// <summary>
    /// Gets all available notification types.
    /// </summary>
    [HttpGet("notification-types")]
    public IActionResult GetNotificationTypes()
    {
        var list = Enum.GetValues<NotificationType>()
            .Select(e => new
            {
                Value = (int)e,
                DisplayName = e.ToString()
            })
            .ToList();

        return Ok(list);
    }

    /// <summary>
    /// Gets all available notification entity types.
    /// </summary>
    [HttpGet("notification-entity-types")]
    public IActionResult GetNotificationEntityTypes()
    {
        var list = Enum.GetValues<NotificationEntityType>()
            .Select(e => new
            {
                Value = (int)e,
                DisplayName = e.ToString()
            })
            .ToList();

        return Ok(list);
    }

    /// <summary>
    /// Gets all available entity roles.
    /// </summary>
    [HttpGet("entity-roles")]
    public IActionResult GetEntityRoles()
    {
        var list = Enum.GetValues<EntityRole>()
            .Select(e => new
            {
                Value = (int)e,
                DisplayName = e.ToString()
            })
            .ToList();

        return Ok(list);
    }

    /// <summary>
    /// Gets all supported device platforms.
    /// </summary>
    [HttpGet("device-platforms")]
    public IActionResult GetDevicePlatforms()
    {
        var list = Enum.GetValues<DevicePlatform>()
            .Select(e => new
            {
                Value = (int)e,
                DisplayName = e.ToString()
            })
            .ToList();

        return Ok(list);
    }

    /// <summary>
    /// Gets all available answer types.
    /// </summary>
    [HttpGet("answer-types")]
    public IActionResult GetAnswerTypes()
    {
        var list = Enum.GetValues<AnswerType>()
            .Select(e => new
            {
                Value = (int)e,
                DisplayName = e.ToString()
            })
            .ToList();

        return Ok(list);
    }
}