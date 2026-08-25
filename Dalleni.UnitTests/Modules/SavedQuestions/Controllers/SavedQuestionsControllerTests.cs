using Dalleni.API.Controllers;
using Dalleni.Application.Features.SavedQuestions.Commands.AddSavedQuestion;
using Dalleni.Application.Features.SavedQuestions.Commands.DeleteSavedQuestion;
using Dalleni.Application.Features.SavedQuestions.Queries.GetUserSavedQuestion;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.SavedQuestions.Controllers;

public class SavedQuestionsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetAllForUserasync_SendsQueryWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok<IEnumerable<SavedQuestion>>(Array.Empty<SavedQuestion>());
        _mediator.Setup(x => x.Send(It.IsAny<GetUserSavedQuestionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new SavedQuestionsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.GetAllForUserasync();

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task CreateAsync_SendsProvidedCommand()
    {
        var response = ResponseFactory.Ok<SavedQuestion>(null!);
        _mediator.Setup(x => x.Send(It.IsAny<AddSavedQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new SavedQuestionsController(_mediator.Object);

        var result = await controller.CreateAsync(new AddSavedQuestionCommand(Guid.NewGuid(), Guid.NewGuid()));

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task RemoveAsync_SendsDeleteCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteSavedQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new SavedQuestionsController(_mediator.Object);

        var result = await controller.RemoveAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }
}

