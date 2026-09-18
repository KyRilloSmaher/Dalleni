
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
        _responseHandlerMock
            .Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) =>
                ResponseFactory.Ok(b, msg));

        _responseHandlerMock
            .Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) =>
                ResponseFactory.NotFound<bool>(msg));

        _responseHandlerMock
            .Setup(x => x.BadRequest<bool>(It.IsAny<string>()))
            .Returns((string msg) =>
                ResponseFactory.BadRequest<bool>(msg));
    }

    // =========================================================
    // New Vote
    // =========================================================

    [Fact]
    public async Task Handle_NewUpvote_AppliesVoteAndReturnsSuccess()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Upvote);

        SetupAnswer(answer);
        SetupNoExistingVote(
            voterUserId,
            answer.Id);

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.True(response.Succeeded);

        Assert.Equal(1, answer.UpVotes);
        Assert.Equal(0, answer.DownVotes);
        Assert.Equal(
            ScoreRules.Upvote,
            answer.Score);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.Is<Vote>(v =>
                    v.UserId == voterUserId &&
                    v.AnswerId == answer.Id &&
                    v.Type == VoteType.Upvote)),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NewDownvote_AppliesVoteAndReturnsSuccess()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Downvote);

        SetupAnswer(answer);
        SetupNoExistingVote(
            voterUserId,
            answer.Id);

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.True(response.Succeeded);

        Assert.Equal(0, answer.UpVotes);
        Assert.Equal(1, answer.DownVotes);
        Assert.Equal(
            -ScoreRules.Downvote,
            answer.Score);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.Is<Vote>(v =>
                    v.UserId == voterUserId &&
                    v.AnswerId == answer.Id &&
                    v.Type == VoteType.Downvote)),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // =========================================================
    // Not Found
    // =========================================================

    [Fact]
    public async Task Handle_AnswerNotFound_ReturnsNotFound()
    {
        var answerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var command = new VoteAnswerCommand(
            answerId,
            voterUserId,
            VoteType.Upvote);

        _unitOfWorkMock
            .Setup(x => x.Answers.GetByIdAsync(
                answerId,
                true))
            .ReturnsAsync((Answer?)null);

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(
            SystemMessages.RECORD_NOT_FOUND,
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForAnswerAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // =========================================================
    // Own Answer
    // =========================================================

    [Fact]
    public async Task Handle_VoteOwnAnswer_ReturnsBadRequest()
    {
        var ownerId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: ownerId);

        var command = new VoteAnswerCommand(
            answer.Id,
            ownerId,
            VoteType.Upvote);

        SetupAnswer(answer);

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.CANNOT_VOTE_OWN_ANSWER,
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForAnswerAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // =========================================================
    // Duplicate Upvote
    // =========================================================

    [Fact]
    public async Task Handle_DuplicateUpvote_ReturnsBadRequest()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        // The answer already contains the existing upvote.
        answer.ApplyVote(VoteType.Upvote);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            answer.Id,
            VoteType.Upvote);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Upvote);

        SetupAnswer(answer);
        SetupExistingVote(
            voterUserId,
            answer.Id,
            existingVote);

        var initialScore = answer.Score;

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.AlREADY_VOTED,
            response.Message);

        Assert.Equal(1, answer.UpVotes);
        Assert.Equal(0, answer.DownVotes);
        Assert.Equal(initialScore, answer.Score);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // =========================================================
    // Duplicate Downvote
    // =========================================================

    [Fact]
    public async Task Handle_DuplicateDownvote_ReturnsBadRequest()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        // The answer already contains the existing downvote.
        answer.ApplyVote(VoteType.Downvote);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            answer.Id,
            VoteType.Downvote);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Downvote);

        SetupAnswer(answer);
        SetupExistingVote(
            voterUserId,
            answer.Id,
            existingVote);

        var initialScore = answer.Score;

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.AlREADY_VOTED,
            response.Message);

        Assert.Equal(0, answer.UpVotes);
        Assert.Equal(1, answer.DownVotes);
        Assert.Equal(initialScore, answer.Score);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // =========================================================
    // Change Upvote -> Downvote
    // =========================================================

    [Fact]
    public async Task Handle_ChangeUpvoteToDownvote_UpdatesVoteCountsAndScore()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        // The answer already contains the existing upvote.
        answer.ApplyVote(VoteType.Upvote);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            answer.Id,
            VoteType.Upvote);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Downvote);

        SetupAnswer(answer);
        SetupExistingVote(
            voterUserId,
            answer.Id,
            existingVote);

        var initialScore = answer.Score;

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.True(response.Succeeded);

        Assert.Equal(0, answer.UpVotes);
        Assert.Equal(1, answer.DownVotes);

        Assert.Equal(
            initialScore
            - ScoreRules.Upvote
            - ScoreRules.Downvote,
            answer.Score);

        Assert.Equal(
            VoteType.Downvote,
            existingVote.Type);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // =========================================================
    // Change Downvote -> Upvote
    // =========================================================

    [Fact]
    public async Task Handle_ChangeDownvoteToUpvote_UpdatesVoteCountsAndScore()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        // The answer already contains the existing downvote.
        answer.ApplyVote(VoteType.Downvote);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            answer.Id,
            VoteType.Downvote);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Upvote);

        SetupAnswer(answer);
        SetupExistingVote(
            voterUserId,
            answer.Id,
            existingVote);

        var initialScore = answer.Score;

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.True(response.Succeeded);

        Assert.Equal(1, answer.UpVotes);
        Assert.Equal(0, answer.DownVotes);

        Assert.Equal(
            initialScore
            + ScoreRules.Downvote
            + ScoreRules.Upvote,
            answer.Score);

        Assert.Equal(
            VoteType.Upvote,
            existingVote.Type);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // =========================================================
    // Save Changes
    // =========================================================

    [Fact]
    public async Task Handle_NewVote_SavesChangesOnce()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Upvote);

        SetupAnswer(answer);
        SetupNoExistingVote(
            voterUserId,
            answer.Id);

        var handler = CreateHandler();

        await handler.Handle(
            command,
            CancellationToken.None);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ChangedVote_DoesNotAddNewVote()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        answer.ApplyVote(VoteType.Upvote);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            answer.Id,
            VoteType.Upvote);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Downvote);

        SetupAnswer(answer);
        SetupExistingVote(
            voterUserId,
            answer.Id,
            existingVote);

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.True(response.Succeeded);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // =========================================================
    // Exceptions
    // =========================================================

    [Fact]
    public async Task Handle_RepositoryThrowsException_ReturnsBadRequest()
    {
        var answerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        _unitOfWorkMock
            .Setup(x => x.Answers.GetByIdAsync(
                answerId,
                true))
            .ThrowsAsync(
                new Exception("Database error"));

        var command = new VoteAnswerCommand(
            answerId,
            voterUserId,
            VoteType.Upvote);

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(
            "Database error",
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesThrowsException_ReturnsBadRequest()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Upvote);

        SetupAnswer(answer);
        SetupNoExistingVote(
            voterUserId,
            answer.Id);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new Exception("Save failed"));

        var handler = CreateHandler();

        var response = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(
            "Save failed",
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // =========================================================
    // Cancellation Token
    // =========================================================

    [Fact]
    public async Task Handle_PassesCancellationTokenToVoteRepository()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        var cancellationToken =
            new CancellationTokenSource().Token;

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Upvote);

        SetupAnswer(answer);

        _unitOfWorkMock
            .Setup(x => x.Votes.GetUserVoteForAnswerAsync(
                voterUserId,
                answer.Id,
                true,
                cancellationToken))
            .ReturnsAsync((Vote?)null);

        var handler = CreateHandler();

        await handler.Handle(
            command,
            cancellationToken);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForAnswerAsync(
                voterUserId,
                answer.Id,
                true,
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToSaveChanges()
    {
        var answerOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var answer = EndpointTestData.Answer(
            userId: answerOwnerId);

        var cancellationToken =
            new CancellationTokenSource().Token;

        var command = new VoteAnswerCommand(
            answer.Id,
            voterUserId,
            VoteType.Upvote);

        SetupAnswer(answer);
        SetupNoExistingVote(
            voterUserId,
            answer.Id);

        var handler = CreateHandler();

        await handler.Handle(
            command,
            cancellationToken);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                cancellationToken),
            Times.Once);
    }

    // =========================================================
    // Helpers
    // =========================================================

    private void SetupAnswer(Answer answer)
    {
        _unitOfWorkMock
            .Setup(x => x.Answers.GetByIdAsync(
                answer.Id,
                true))
            .ReturnsAsync(answer);
    }

    private void SetupNoExistingVote(
        Guid userId,
        Guid answerId)
    {
        _unitOfWorkMock
            .Setup(x => x.Votes.GetUserVoteForAnswerAsync(
                userId,
                answerId,
                true,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vote?)null);
    }

    private void SetupExistingVote(
        Guid userId,
        Guid answerId,
        Vote vote)
    {
        _unitOfWorkMock
            .Setup(x => x.Votes.GetUserVoteForAnswerAsync(
                userId,
                answerId,
                true,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(vote);
    }

    private VoteAnswerHandler CreateHandler()
    {
        return new VoteAnswerHandler(
            _unitOfWorkMock.Object,
            _responseHandlerMock.Object,
            _loggerMock.Object);
    }
}
