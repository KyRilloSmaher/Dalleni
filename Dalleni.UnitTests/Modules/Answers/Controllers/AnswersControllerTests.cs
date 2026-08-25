using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Requests.Answers;
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Application.Features.Answers.Commands.CreateAnswer;
using Dalleni.Application.Features.Answers.Commands.DeleteAnswer;
using Dalleni.Application.Features.Answers.Commands.UpdateAnswer;
using Dalleni.Application.Features.Answers.Queries;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.Answers.Controllers;

public class AnswersControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetByQuestionIdAsync_SendsQueryAndReturnsOk()
    {
        var response = ResponseFactory.Ok<IEnumerable<AnswerDto>>(new[] { EndpointTestData.AnswerDto() });
        _mediator.Setup(x => x.Send(It.IsAny<GetAnswersByQuestionIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new AnswersController(_mediator.Object);

        var result = await controller.GetByQuestionIdAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<GetAnswersByQuestionIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_SendsCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(Guid.NewGuid());
        _mediator.Setup(x => x.Send(It.IsAny<CreateAnswerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new AnswersController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.CreateAsync(new CreateAnswerRequestDto { QuestionId = Guid.NewGuid(), Content = "Answer" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<CreateAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_SendsCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<UpdateAnswerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new AnswersController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.UpdateAsync(Guid.NewGuid(), new UpdateAnswerRequestDto { Content = "Updated" });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<UpdateAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SendsCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteAnswerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new AnswersController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.DeleteAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<DeleteAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

