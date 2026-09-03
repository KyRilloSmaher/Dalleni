using Dalleni.Application.Features.Votes.Commands.VoteQuestion;
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

public class VoteQuestionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<VoteQuestionHandler>> _loggerMock = new();

    public VoteQuestionHandlerTests()
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
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();
        var question = EndpointTestData.Question(userId: questionOwnerId);
        var command = new VoteQuestionCommand(question.Id, voterUserId, VoteType.Upvote);

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);
        _unitOfWorkMock.Setup(x => x.Votes.GetUserVoteForQuestionAsync(voterUserId, question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vote?)null);

        var handler = new VoteQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.Equal(1, question.UpVotes);
        _unitOfWorkMock.Verify(x => x.Votes.AddAsync(It.IsAny<Vote>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_VoteOwnQuestion_ReturnsBadRequest()
    {
        var ownerId = Guid.NewGuid();
        var question = EndpointTestData.Question(userId: ownerId);
        var command = new VoteQuestionCommand(question.Id, ownerId, VoteType.Upvote);

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var handler = new VoteQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.CANNOT_VOTE_OWN_QUESTION, response.Message);
    }

    [Fact]
    public async Task Handle_DuplicateVoteSameType_ReturnsBadRequest()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();
        var question = EndpointTestData.Question(userId: questionOwnerId);
        var existingVote = EndpointTestData.QuestionVote(voterUserId, question.Id, VoteType.Upvote);
        var command = new VoteQuestionCommand(question.Id, voterUserId, VoteType.Upvote);

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);
        _unitOfWorkMock.Setup(x => x.Votes.GetUserVoteForQuestionAsync(voterUserId, question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingVote);

        var handler = new VoteQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.AlREADY_VOTED, response.Message);
    }
}
