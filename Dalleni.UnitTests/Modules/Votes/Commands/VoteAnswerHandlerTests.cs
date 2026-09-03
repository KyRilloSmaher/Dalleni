using Dalleni.Application.Features.Votes.Commands.VoteAnswer;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Votes.Commands;

public class VoteAnswerHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<VoteAnswerHandler>> _loggerMock = new();

    public VoteAnswerHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<bool>(msg));
        _responseHandlerMock.Setup(x => x.BadRequest<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<bool>(msg));
    }

    [Fact]
    public async Task Handle_NewUpvote_AppliesVoteAndReturnsSuccess()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();
        var answer = EndpointTestData.Answer(userId: answerOwnerId);
        var command = new VoteAnswerCommand(answer.Id, voterUserId, VoteType.Upvote);

        _unitOfWorkMock.Setup(x => x.Answers.GetByIdAsync(answer.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);
        _unitOfWorkMock.Setup(x => x.Votes.GetUserVoteForAnswerAsync(voterUserId, answer.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vote?)null);

        var handler = new VoteAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.Equal(1, answer.UpVotes);
        _unitOfWorkMock.Verify(x => x.Votes.AddAsync(It.IsAny<Vote>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_VoteOwnAnswer_ReturnsBadRequest()
    {
        var ownerId = Guid.NewGuid();
        var answer = EndpointTestData.Answer(userId: ownerId);
        var command = new VoteAnswerCommand(answer.Id, ownerId, VoteType.Upvote);

        _unitOfWorkMock.Setup(x => x.Answers.GetByIdAsync(answer.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var handler = new VoteAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.CANNOT_VOTE_OWN_ANSWER, response.Message);
    }
}
