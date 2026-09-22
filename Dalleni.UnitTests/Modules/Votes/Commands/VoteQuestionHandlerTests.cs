

using Dalleni.Application.DTOs.Responses.Votes;
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
            .Setup(x => x.Success(
                It.IsAny<NewVoteResponse>(),
                It.IsAny<string>()))
            .Returns((NewVoteResponse value, string message) =>
                ResponseFactory.Ok(value, message));

        _responseHandlerMock
            .Setup(x => x.NotFound<NewVoteResponse>(
                It.IsAny<string>()))
            .Returns((string message) =>
                ResponseFactory.NotFound<NewVoteResponse>(message));

        _responseHandlerMock
            .Setup(x => x.BadRequest<NewVoteResponse>(
                It.IsAny<string>()))
            .Returns((string message) =>
                ResponseFactory.BadRequest<NewVoteResponse>(message));
    }

    // ============================================================
    // New Vote
    // ============================================================

    [Fact]
    public async Task Handle_NewUpvote_AppliesVoteAndReturnsSuccess()
    {
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
        SetupQuestionAfterCommit(question);

        var initialScore = question.Score;

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

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
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NewDownvote_AppliesVoteAndReturnsSuccess()
    {
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
        SetupQuestionAfterCommit(question);

        var initialScore = question.Score;
        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

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
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // Question Not Found
    // ============================================================

    [Fact]
    public async Task Handle_QuestionNotFound_ReturnsNotFound()
    {
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

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

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
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ============================================================
    // Own Question
    // ============================================================

    [Fact]
    public async Task Handle_VoteOwnQuestion_ReturnsBadRequest()
    {
        var ownerId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: ownerId);

        var command = new VoteQuestionCommand(
            question.Id,
            ownerId,
            VoteType.Upvote);

        SetupQuestion(question);

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

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
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    // ============================================================
    // Duplicate Votes
    // ============================================================

    [Fact]
    public async Task Handle_DuplicateUpvote_ReturnsBadRequest()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        question.ApplyVote(VoteType.Upvote);
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

        var initialScore = question.Score;

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.AlREADY_VOTED,
            response.Message);

        Assert.Equal(1, question.UpVotes);
        Assert.Equal(0, question.DownVotes);
        Assert.Equal(initialScore, question.Score);

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
            Times.Never);
    }

    [Fact]
    public async Task Handle_DuplicateDownvote_ReturnsBadRequest()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        question.ApplyVote(VoteType.Downvote);

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

        var initialScore = question.Score;

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);

        Assert.Equal(
            SystemMessages.AlREADY_VOTED,
            response.Message);

        Assert.Equal(0, question.UpVotes);
        Assert.Equal(1, question.DownVotes);
        Assert.Equal(initialScore, question.Score);

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
            Times.Never);
    }

    // ============================================================
    // Change Vote
    // ============================================================

    [Fact]
    public async Task Handle_ChangeUpvoteToDownvote_UpdatesVoteCountsAndScore()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

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
        SetupQuestionAfterCommit(question);

        var initialScore = question.Score;

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

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
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ChangeDownvoteToUpvote_UpdatesVoteCountsAndScore()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

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
        SetupQuestionAfterCommit(question);

        var initialScore = question.Score;

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

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
            x => x.Votes.AddAsync(
                It.IsAny<Vote>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // Exception Handling
    // ============================================================

    [Fact]
    public async Task Handle_RepositoryThrows_ReturnsBadRequest()
    {
        var questionId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        const string exceptionMessage = "Database error";

        _unitOfWorkMock
            .Setup(x => x.Questions.GetByIdAsync(
                questionId,
                true))
            .ThrowsAsync(new Exception(exceptionMessage));

        var command = new VoteQuestionCommand(
            questionId,
            voterUserId,
            VoteType.Upvote);

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(exceptionMessage, response.Message);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_SaveChangesThrows_ReturnsBadRequest()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        SetupQuestion(question);
        SetupNoExistingVote(
            voterUserId,
            question.Id);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Save failed"));

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        var response = await CreateHandler().Handle(
            command,
            CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal("Save failed", response.Message);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    // ============================================================
    // Cancellation Token
    // ============================================================

    [Fact]
    public async Task Handle_PassesCancellationTokenToVoteLookup()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var cancellationToken =
            new CancellationTokenSource().Token;

        SetupQuestion(question);

        _unitOfWorkMock
            .Setup(x => x.Votes.GetUserVoteForQuestionAsync(
                voterUserId,
                question.Id,
                true,
                cancellationToken))
            .ReturnsAsync((Vote?)null);

        SetupQuestionAfterCommit(question);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        var response = await CreateHandler().Handle(
            command,
            cancellationToken);

        Assert.True(response.Succeeded);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetUserVoteForQuestionAsync(
                voterUserId,
                question.Id,
                true,
                cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToSaveChanges()
    {
        var questionOwnerId = Guid.NewGuid();
        var voterUserId = Guid.NewGuid();

        var question = EndpointTestData.Question(
            userId: questionOwnerId);

        var cancellationToken =
            new CancellationTokenSource().Token;

        SetupQuestion(question);
        SetupNoExistingVote(
            voterUserId,
            question.Id);
        SetupQuestionAfterCommit(question);

        var command = new VoteQuestionCommand(
            question.Id,
            voterUserId,
            VoteType.Upvote);

        await CreateHandler().Handle(
            command,
            cancellationToken);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                cancellationToken),
            Times.Once);
    }

    // ============================================================
    // Helpers
    // ============================================================

    private void SetupQuestion(Question question)
    {
        _unitOfWorkMock
            .Setup(x => x.Questions.GetByIdAsync(
                question.Id,
                true))
            .ReturnsAsync(question);
    }

    private void SetupQuestionAfterCommit(Question question)
    {
        _unitOfWorkMock
            .Setup(x => x.Questions.GetByIdAsync(
                question.Id,
                false))
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