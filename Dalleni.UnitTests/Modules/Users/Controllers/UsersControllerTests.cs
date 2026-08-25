using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Application.Features.Users.Commands.DeleteAccount;
using Dalleni.Application.Features.Users.Commands.RestoreAccount;
using Dalleni.Application.Features.Users.Commands.UpdateProfile;
using Dalleni.Application.Features.Users.Commands.UpdateRrofileImage;
using Dalleni.Application.Features.Users.Queries.GetTopContributors;
using Dalleni.Application.Features.Users.Queries.GetTopUsers;
using Dalleni.Application.Features.Users.Queries.GetUserByEmail;
using Dalleni.Application.Features.Users.Queries.GetUserById;
using Dalleni.Application.Features.Users.Queries.GetUserStats;
using Dalleni.Application.Features.Users.Queries.SearchUserByName;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.Users.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetUserByIdAsync_SendsQuery()
    {
        var response = ResponseFactory.Ok(EndpointTestData.UserResponse());
        _mediator.Setup(x => x.Send(It.IsAny<GetUserByIdlQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.GetUserByIdAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetCurrentUserAsync_SendsQueryWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(EndpointTestData.UserResponse());
        _mediator.Setup(x => x.Send(It.IsAny<GetUserByIdlQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.GetCurrentUserAsync();

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetUserByEmailAsync_SendsQuery()
    {
        var response = ResponseFactory.Ok(EndpointTestData.UserResponse());
        _mediator.Setup(x => x.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.GetUserByEmailAsync("test@example.com");

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task SearchUsersAsync_SendsSearchQuery()
    {
        var response = ResponseFactory.Ok(new List<UserResponseDto> { EndpointTestData.UserResponse() });
        _mediator.Setup(x => x.Send(It.IsAny<SearchUsersByNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.SearchUsersAsync("test");

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task UpdateUserProfileAsync_SendsUpdateProfileCommand()
    {
        var response = ResponseFactory.Ok(EndpointTestData.UserResponse());
        _mediator.Setup(x => x.Send(It.IsAny<UpdateProfileCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.UpdateUserProfileAsync(new UpdateUserAccount { Id = Guid.NewGuid(), FirstName = "Test" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task UpdateUserProfileImageAsync_SendsUpdateProfileImageCommand()
    {
        var response = ResponseFactory.Ok("https://example.com/image.png");
        _mediator.Setup(x => x.Send(It.IsAny<UpdateProfileImageCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.UpdateUserProfileImageAsync(new UpdateUserProfileImage { Id = Guid.NewGuid(), ProfileImage = null! });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task RestoreUserAsync_SendsRestoreCommand()
    {
        var response = ResponseFactory.Ok<TokenReponseDto>(EndpointTestData.TokenResponse());
        _mediator.Setup(x => x.Send(It.IsAny<RestoreAccountCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.RestoreUserAsync(new RestoreAccountRequest { Email = "test@example.com", PhoneNumber = "01000000000" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task DeleteUserAsync_SendsDeleteCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteAccountCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.DeleteUserAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetTopUsersAsync_SendsTopUsersQuery()
    {
        var response = ResponseFactory.Ok<IEnumerable<UserResponseDto>>(new[] { EndpointTestData.UserResponse() });
        _mediator.Setup(x => x.Send(It.IsAny<GetTopUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.GetTopUsersAsync(5);

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetTopContributorsAsync_SendsTopContributorsQuery()
    {
        var response = ResponseFactory.Ok<IEnumerable<UserResponseDto>>(new[] { EndpointTestData.UserResponse() });
        _mediator.Setup(x => x.Send(It.IsAny<GetTopContributorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.GetTopContributorsAsync(5);

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetUserStatsAsync_SendsStatsQuery()
    {
        var response = ResponseFactory.Ok(EndpointTestData.UserResponse());
        _mediator.Setup(x => x.Send(It.IsAny<GetUserStatsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new UsersController(_mediator.Object);

        var result = await controller.GetUserStatsAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }
}

