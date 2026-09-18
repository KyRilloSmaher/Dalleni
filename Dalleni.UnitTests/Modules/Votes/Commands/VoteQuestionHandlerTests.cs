
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
        _responseHandlerMock
            .Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool value, string message) =>
                ResponseFactory.Ok(value, message));

        _responseHandlerMock
            .Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string message) =>
                ResponseFactory.NotFound<bool>(message));

        _responseHandlerMock
            .Setup(x => x.BadRequest<bool>(It.IsAny<string>()))
            .Returns((string message) =>
                ResponseFactory.BadRequest<bool>(message));
    }

    // ============================================================
    // New Vote
    // ============================================================

    [Fact]
    public async Task Handle_NewUpvote_AppliesVoteAndReturnsSuccess()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        SetupQuestion(question);
        SetupNoExistingVote(voterUserId, question.Id);

        var initialScore = question.Score;

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);

        Assert.Equal(1, question.UpVotes);
        Assert.Equal(0, question.DownVotes);

        Assert.Equal(
            initialScore + ScoreRules.Upvote,
            question.Score);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.Is<Vote>(v =>
                    v.UserId == voterUserId &&
                    v.QuestionId == question.Id &&
                    v.Type == VoteType.Upvote)),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NewDownvote_AppliesVoteAndReturnsSuccess()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Downvote);

        SetupQuestion(question);
        SetupNoExistingVote(voterUserId, question.Id);

        var initialScore = question.Score;

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);

        Assert.Equal(0, question.UpVotes);
        Assert.Equal(1, question.DownVotes);

        Assert.Equal(
            initialScore - ScoreRules.Downvote,
            question.Score);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.Is<Vote>(v =>
                    v.UserId == voterUserId &&
                    v.QuestionId == question.Id &&
                    v.Type == VoteType.Downvote)),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // Question Validation
    // ============================================================

    [Fact]
    public async Task Handle_QuestionNotFound_ReturnsNotFound()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var command = new VoteQuestionCommand(
            questionId,
            voterUserId,
            VoteType.Upvote);

        _unitOfWorkMock
            .Setup(x => x.Questions.GetByIdAsync(
                questionId,
                true))
            .ReturnsAsync((Question?)null);

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.RECORD_NOT_FOUND,
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForQuestionAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_VoteOwnQuestion_ReturnsBadRequest()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: ownerId);

        var command = new VoteQuestionCommand(
            question.Id,
            ownerId,
            VoteType.Upvote);

        SetupQuestion(question);

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.CANNOT_VOTE_OWN_QUESTION,
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForQuestionAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ============================================================
    // Duplicate Votes
    // ============================================================

    [Fact]
    public async Task Handle_DuplicateUpvote_ReturnsBadRequest()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            question.Id,
            VoteType.Upvote);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        SetupQuestion(question);
        SetupExistingVote(
            voterUserId,
            question.Id,
            existingVote);

        var initialUpVotes = question.UpVotes;
        var initialDownVotes = question.DownVotes;
        var initialScore = question.Score;

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.AlREADY_VOTED,
            response.Message);

        Assert.Equal(initialUpVotes, question.UpVotes);
        Assert.Equal(initialDownVotes, question.DownVotes);
        Assert.Equal(initialScore, question.Score);

        Assert.Equal(
            VoteType.Upvote,
            existingVote.Type);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_DuplicateDownvote_ReturnsBadRequest()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            question.Id,
            VoteType.Downvote);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Downvote);

        SetupQuestion(question);
        SetupExistingVote(
            voterUserId,
            question.Id,
            existingVote);

        var initialUpVotes = question.UpVotes;
        var initialDownVotes = question.DownVotes;
        var initialScore = question.Score;

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.AlREADY_VOTED,
            response.Message);

        Assert.Equal(initialUpVotes, question.UpVotes);
        Assert.Equal(initialDownVotes, question.DownVotes);
        Assert.Equal(initialScore, question.Score);

        Assert.Equal(
            VoteType.Downvote,
            existingVote.Type);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ============================================================
    // Change Vote
    // ============================================================
    [Fact]
    public async Task Handle_ChangeUpvoteToDownvote_UpdatesVoteCountsAndScore()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        // The question already contains the existing upvote.
        question.ApplyVote(VoteType.Upvote);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            question.Id,
            VoteType.Upvote);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Downvote);

        SetupQuestion(question);

        SetupExistingVote(
            voterUserId,
            question.Id,
            existingVote);

        var initialScore = question.Score;

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);

        Assert.Equal(0, question.UpVotes);
        Assert.Equal(1, question.DownVotes);

        Assert.Equal(
            initialScore
            - ScoreRules.Upvote
            - ScoreRules.Downvote,
            question.Score);

        Assert.Equal(
            VoteType.Downvote,
            existingVote.Type);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ChangeDownvoteToUpvote_UpdatesVoteCountsAndScore()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        // The question already contains the existing downvote.
        question.ApplyVote(VoteType.Downvote);

        var existingVote = EndpointTestData.QuestionVote(
            voterUserId,
            question.Id,
            VoteType.Downvote);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        SetupQuestion(question);

        SetupExistingVote(
            voterUserId,
            question.Id,
            existingVote);

        var initialScore = question.Score;

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);

        Assert.Equal(1, question.UpVotes);
        Assert.Equal(0, question.DownVotes);

        Assert.Equal(
            initialScore
            + ScoreRules.Downvote
            + ScoreRules.Upvote,
            question.Score);

        Assert.Equal(
            VoteType.Upvote,
            existingVote.Type);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }


    // ============================================================
    // Persistence / Cancellation
    // ============================================================

    [Fact]
    public async Task Handle_SuccessfulNewVote_PassesCancellationTokenToSaveChanges()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        SetupQuestion(question);
        SetupNoExistingVote(voterUserId, question.Id);

        var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            cancellationToken);

        // Assert
        Assert.True(response.Succeeded);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_SuccessfulVote_PassesCancellationTokenToVoteLookup()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        SetupQuestion(question);

        var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        _unitOfWorkMock
            .Setup(x => x.Votes.GetUserVoteForQuestionAsync(
                voterUserId,
                question.Id,
                true,
                cancellationToken))
            .ReturnsAsync((Vote?)null);

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            cancellationToken);

        // Assert
        Assert.True(response.Succeeded);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForQuestionAsync(
                voterUserId,
                question.Id,
                true,
                cancellationToken),
            Times.Once);
    }

    // ============================================================
    // Exception Handling
    // ============================================================

    [Fact]
    public async Task Handle_WhenRepositoryThrows_ReturnsBadRequest()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        var exceptionMessage = "Database connection failed.";

        _unitOfWorkMock
            .Setup(x => x.Questions.GetByIdAsync(
                question.Id,
                true))
            .ThrowsAsync(
                new Exception(exceptionMessage));

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);

        Assert.Equal(
            exceptionMessage,
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForQuestionAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSaveChangesThrows_ReturnsBadRequest()
    {
        // Arrange
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        SetupQuestion(question);
        SetupNoExistingVote(voterUserId, question.Id);

        var exceptionMessage = "Unable to save changes.";

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(
                new Exception(exceptionMessage));

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);

        Assert.Equal(
            exceptionMessage,
            response.Message);

        Assert.Equal(1, question.UpVotes);

        Assert.Equal(
            ScoreRules.Upvote,
            question.Score);

        _unitOfWorkMock.Verify(
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // Helper Methods
    // ============================================================

    private void SetupQuestion(Question question)
    {
        _unitOfWorkMock
            .Setup(x => x.Questions.GetByIdAsync(
                question.Id,
                true))
            .ReturnsAsync(question);
    }

    private void SetupNoExistingVote(
        Guid userId,
        Guid questionId)
    {
        _unitOfWorkMock
            .Setup(x => x.Votes.GetUserVoteForQuestionAsync(
                userId,
                questionId,
                true,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Vote?)null);
    }

    private void SetupExistingVote(
        Guid userId,
        Guid questionId,
        Vote vote)
    {
        _unitOfWorkMock
            .Setup(x => x.Votes.GetUserVoteForQuestionAsync(
                userId,
                questionId,
                true,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(vote);
    }

    private VoteQuestionHandler CreateHandler()
    {
        return new VoteQuestionHandler(
            _unitOfWorkMock.Object,
            _responseHandlerMock.Object,
            _loggerMock.Object);
    }
}
