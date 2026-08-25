using Dalleni.API.Controllers;
using Dalleni.Application.Features.Votes.Commands.VoteAnswer;
using Dalleni.Application.Features.Votes.Commands.VoteQuestion;
using Dalleni.Domin.Enums;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.Votes.Controllers;

public class VotesControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task VoteQuestionAsync_SendsVoteQuestionCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<VoteQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new VotesController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.VoteQuestionAsync(Guid.NewGuid(), VoteType.Upvote);

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<VoteQuestionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task VoteAnswerAsync_SendsVoteAnswerCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<VoteAnswerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new VotesController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.VoteAnswerAsync(Guid.NewGuid(), VoteType.Downvote);

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<VoteAnswerCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
