using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dalleni.UnitTests.Shared.Controllers;

public static class ControllerTestHelper
{
    public static void SetUser(ControllerBase controller, Guid userId)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }, "TestAuth"))
            }
        };
    }

    public static void SetHttpContext(ControllerBase controller, HttpContext httpContext)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }
}

