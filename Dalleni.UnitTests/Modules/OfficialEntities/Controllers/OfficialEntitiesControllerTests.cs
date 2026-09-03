using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Requests.OfficialEntities;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Commands.DeleteOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Commands.RestoreOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Commands.UpdateOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Commands.VerifyOfficialEntity;
using Dalleni.Application.Features.OfficialEntities.Create;
using Dalleni.Application.Features.OfficialEntities.Members.AcceptInvitation;
using Dalleni.Application.Features.OfficialEntities.Members.Invite;
using Dalleni.Application.Features.OfficialEntities.Queries.GetAllOfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Queries.GetMyOfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Queries.GetOfficialEntityById;
using Dalleni.Application.Features.OfficialEntities.Queries.GetVerifiedOfficialEntities;
using Dalleni.Application.Features.OfficialEntities.Queries.SearchOfficialEntities;
using Dalleni.Domin.Enums;
using Dalleni.Domin.ResponsePattern;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.OfficialEntities.Controllers;

public class OfficialEntitiesControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetByIdAsync_SendsQueryWithProvidedId()
    {
        var response = ResponseFactory.Ok<OfficialEntityDto>(new OfficialEntityDto());
        _mediator.Setup(x => x.Send(It.IsAny<GetOfficialEntityByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var id = Guid.NewGuid();

        var result = await controller.GetByIdAsync(id);

        _mediator.Verify(x => x.Send(It.Is<GetOfficialEntityByIdQuery>(q => q.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetAllAsync_SendsQueryWithPagedRequest()
    {
        var response = ResponseFactory.Ok<PaginatedResult<OfficialEntityDto>>(new PaginatedResult<OfficialEntityDto>());
        _mediator.Setup(x => x.Send(It.IsAny<GetAllOfficialEntitiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var request = new PagedRequest();

        var result = await controller.GetAllAsync(request);

        _mediator.Verify(x => x.Send(It.Is<GetAllOfficialEntitiesQuery>(q => q.request == request), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetVerifiedAsync_SendsQueryWithPagedRequest()
    {
        var response = ResponseFactory.Ok<PaginatedResult<OfficialEntityDto>>(new PaginatedResult<OfficialEntityDto>());
        _mediator.Setup(x => x.Send(It.IsAny<GetVerifiedOfficialEntitiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var request = new PagedRequest();

        var result = await controller.GetVerifiedAsync(request);

        _mediator.Verify(x => x.Send(It.Is<GetVerifiedOfficialEntitiesQuery>(q => q.request == request), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task SearchAsync_SendsQueryWithKeyword()
    {
        var response = ResponseFactory.Ok<IEnumerable<OfficialEntityDto>>(Array.Empty<OfficialEntityDto>());
        _mediator.Setup(x => x.Send(It.IsAny<SearchOfficialEntitiesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var keyword = "test";

        var result = await controller.SearchAsync(keyword);

        _mediator.Verify(x => x.Send(It.Is<SearchOfficialEntitiesQuery>(q => q.Keyword == keyword), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task GetMyEntitiesAsync_SendsQueryWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok<OfficialEntityDto>(new OfficialEntityDto());
        _mediator.Setup(x => x.Send(It.IsAny<GetMyOfficialEntityQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var userId = Guid.NewGuid();
        ControllerTestHelper.SetUser(controller, userId);

        var result = await controller.GetMyEntitiesAsync();

        _mediator.Verify(x => x.Send(It.Is<GetMyOfficialEntityQuery>(q => q.userId == userId), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    // [Fact]
    // public async Task InviteMemberAsync_SendsCommandWithAuthenticatedUser()
    // {
    //     var response = ResponseFactory.Ok(true);
    //     _mediator.Setup(x => x.Send(It.IsAny<InviteEntityMemberCommand>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(response);
    //     var controller = new OfficialEntitiesController(_mediator.Object);
    //     var userId = Guid.NewGuid();
    //     ControllerTestHelper.SetUser(controller, userId);
    //     var entityId = Guid.NewGuid();
    //     var dto = new InviteEntityMemberRequestDto { Email = "test@test.com", Role = EntityRole.Staff };

    //     var result = await controller.InviteMemberAsync(entityId, dto);

    //     _mediator.Verify(x => x.Send(
    //         It.Is<InviteEntityMemberCommand>(q => 
    //             q.currentuserId == userId && 
    //             q.OfficialEntityId == entityId && 
    //             q.Email == dto.Email && 
    //             q.Role == dto.Role), 
    //         It.IsAny<CancellationToken>()), Times.Once);
    //     Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    // }

    // [Fact]
    // public async Task AcceptInvitationAsync_SendsCommandWithAuthenticatedUser()
    // {
    //     var response = ResponseFactory.Ok(true);
    //     _mediator.Setup(x => x.Send(It.IsAny<AcceptEntityInvitationCommand>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(response);
    //     var controller = new OfficialEntitiesController(_mediator.Object);
    //     var userId = Guid.NewGuid();
    //     ControllerTestHelper.SetUser(controller, userId);
    //     var dto = new AcceptEntityInvitationRequestDto("test-token");

    //     var result = await controller.AcceptInvitationAsync(dto);

    //     _mediator.Verify(x => x.Send(
    //         It.Is<AcceptEntityInvitationCommand>(q => 
    //             q.currentUserId == userId && 
    //             q.Token == dto.Token), 
    //         It.IsAny<CancellationToken>()), Times.Once);
    //     Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    // }

    [Fact]
    public async Task CreateAsync_SendsCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(Guid.NewGuid());
        _mediator.Setup(x => x.Send(It.IsAny<CreateOfficialEntityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var userId = Guid.NewGuid();
        ControllerTestHelper.SetUser(controller, userId);
        var dto = new CreateOfficialEntityRequestDto();

        var result = await controller.CreateAsync(dto);

        _mediator.Verify(x => x.Send(
            It.Is<CreateOfficialEntityCommand>(q => 
                q.currentUserId == userId && 
                q.dto == dto), 
            It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task UpdateAsync_SendsCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<UpdateOfficialEntityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var userId = Guid.NewGuid();
        ControllerTestHelper.SetUser(controller, userId);
        var entityId = Guid.NewGuid();
        var dto = new UpdateOfficialEntityRequestDto();

        var result = await controller.UpdateAsync(entityId, dto);

        _mediator.Verify(x => x.Send(
            It.Is<UpdateOfficialEntityCommand>(q => 
                q.Id == entityId && 
                q.userId == userId && 
                q.Dto == dto), 
            It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task VerifyAsync_SendsCommandWithEntityId()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<VerifyOfficialEntityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());
        var entityId = Guid.NewGuid();

        var result = await controller.VerifyAsync(entityId);

        _mediator.Verify(x => x.Send(It.Is<VerifyOfficialEntityCommand>(q => q.Id == entityId), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task DeleteAsync_SendsCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteOfficialEntityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var userId = Guid.NewGuid();
        ControllerTestHelper.SetUser(controller, userId);
        var entityId = Guid.NewGuid();

        var result = await controller.DeleteAsync(entityId);

        _mediator.Verify(x => x.Send(
            It.Is<DeleteOfficialEntityCommand>(q => 
                q.Id == entityId && 
                q.UserId == userId), 
            It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task RestoreAsync_SendsCommandWithEntityId()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<RestoreOfficialEntityCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new OfficialEntitiesController(_mediator.Object);
        var entityId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        ControllerTestHelper.SetUser(controller, userId);

        var result = await controller.RestoreAsync(entityId);

        _mediator.Verify(x => x.Send(It.Is<RestoreOfficialEntityCommand>(q => q.Id == entityId && q.userId == userId), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }
}
