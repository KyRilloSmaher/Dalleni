using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Application.Features.Authantications.ChangePassword;
using Dalleni.Application.Features.Authantications.ConfirmEmail;
using Dalleni.Application.Features.Authantications.ConfirmOTPForResetPassword;
using Dalleni.Application.Features.Authantications.GooleSignin;
using Dalleni.Application.Features.Authantications.Login;
using Dalleni.Application.Features.Authantications.Logout;
using Dalleni.Application.Features.Authantications.RefreshToken;
using Dalleni.Application.Features.Authantications.ResendEmailConfirmationCode;
using Dalleni.Application.Features.Authantications.ResendOTPForResetPassword;
using Dalleni.Application.Features.Authantications.ResetPasssword;
using Dalleni.Application.Features.Authantications.SendResetPasswordCode;
using Dalleni.Application.Features.Authantications.SignUp;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;

namespace Dalleni.UnitTests.Modules.Authantications.Controllers;

public class AuthenticationControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task SignUpAsync_SendsSignUpCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<SignUpCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.SignUpAsync(new SignUpRequest { Email = "test@example.com" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task LoginAsync_SendsLoginCommand()
    {
        var response = ResponseFactory.Ok<TokenReponseDto>(EndpointTestData.TokenResponse());
        _mediator.Setup(x => x.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.LoginAsync(new LoginRequestDto { Email = "test@example.com", Password = "P@ssw0rd" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task LogoutAsync_SendsLogoutCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<LogoutCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.LogoutAsync();

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task RefreshTokenAsync_SendsRefreshTokenCommand()
    {
        var response = ResponseFactory.Ok<TokenReponseDto>(EndpointTestData.TokenResponse());
        _mediator.Setup(x => x.Send(It.IsAny<RefreshTokenAsyncCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.RefreshTokenAsync(new TokenRequestDto { AccessToken = "access", RefreshToken = "refresh" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task ChangePasswordAsync_SendsChangePasswordCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<ChangePasswordCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.ChangePasswordAsync(new ChangePasswordRequestDto { CurrentPassword = "old", NewPassword = "new" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task SendResetPasswordCodeAsync_SendsCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<SendResetPasswordCodeCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.SendResetPasswordCodeAsync(new ForgetPasswordRequestDto { Email = "test@example.com" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task ReSendResetPasswordCodeAsync_SendsCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<ReSendOTPForResetPasswordCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.ReSendResetPasswordCodeAsync(new ForgetPasswordRequestDto { Email = "test@example.com" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task ConfirmResetPasswordCodeAsync_SendsCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<ConfirmResetPasswordOTPCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.ConfirmResetPasswordCodeAsync(new ConfirmResetPasswordCodeRequest { Email = "test@example.com", Code = "123456" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task ResetPasswordAsync_SendsCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.ResetPasswordAsync(new SetNewPasswordRequestDto { Email = "test@example.com", NewPassword = "P@ssw0rd" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task ConfirmEmailAsync_SendsCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<ConfirmEmailCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.ConfirmEmailAsync(new ConfirmEmailRequest { Email = "test@example.com", Code = "123456" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task ReSendConfirmationEmailAsync_SendsCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<ResendConfirmaionEmailCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var controller = new AuthenticationController(_mediator.Object);

        var result = await controller.ReSendConfirmationEmailAsync(new ReSendConfirmationEmailRequest { Email = "test@example.com" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GoogleSignin_WhenAuthenticationFails_ReturnsBadRequest()
    {
        var authenticationService = new Mock<IAuthenticationService>();
        authenticationService
            .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
            .ReturnsAsync(AuthenticateResult.Fail("failed"));
        var services = new ServiceCollection()
            .AddSingleton(authenticationService.Object)
            .BuildServiceProvider();
        var controller = new AuthenticationController(_mediator.Object);
        ControllerTestHelper.SetHttpContext(controller, new DefaultHttpContext { RequestServices = services });

        var result = await controller.GoogleSignin();

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GoogleSignin_WhenAuthenticationSucceeds_SendsGoogleSigninCommand()
    {
        var response = ResponseFactory.Ok<TokenReponseDto>(EndpointTestData.TokenResponse());
        _mediator.Setup(x => x.Send(It.IsAny<GooleSigninCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);
        var authenticationService = new Mock<IAuthenticationService>();
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Surname, "User"),
            new Claim(ClaimTypes.Name, "Test"),
            new Claim("picture", "https://example.com/image.png"),
            new Claim(ClaimTypes.NameIdentifier, "google-id")
        }, "Google"));
        authenticationService
            .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
            .ReturnsAsync(AuthenticateResult.Success(new AuthenticationTicket(principal, "Cookies")));
        var services = new ServiceCollection()
            .AddSingleton(authenticationService.Object)
            .BuildServiceProvider();
        var controller = new AuthenticationController(_mediator.Object);
        ControllerTestHelper.SetHttpContext(controller, new DefaultHttpContext { RequestServices = services });

        var result = await controller.GoogleSignin();

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<GooleSigninCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginWithGoogle_ReturnsChallengeResult()
    {
        var controller = new AuthenticationController(_mediator.Object)
        {
            Url = Mock.Of<IUrlHelper>(x => x.Action(It.IsAny<UrlActionContext>()) == "https://example.com/google-callback")
        };
        ControllerTestHelper.SetHttpContext(controller, new DefaultHttpContext());

        var result = await controller.LoginWithGoogle();

        Assert.IsType<ChallengeResult>(result);
    }
}

