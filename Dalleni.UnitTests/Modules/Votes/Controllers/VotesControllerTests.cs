 using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Application.Features.Votes.Commands.DeleteVote;
using Dalleni.Application.Features.Votes.Commands.VoteAnswer;
using Dalleni.Application.Features.Votes.Commands.VoteQuestion;
using Dalleni.Application.Features.Votes.Queries.GetUserVotedAnswersQuery;
using Dalleni.Application.Features.Votes.Queries.GetUserVotedQuestionsQuery;
using Dalleni.Domin.Enums;
using Dalleni.Domin.ResponsePattern;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Dalleni.UnitTests.Modules.Votes.Controllers;

public class VotesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    private readonly Guid _userId = Guid.NewGuid();

    private VotesController CreateController()
    {
        var controller = new VotesController(_mediatorMock.Object);

        ControllerTestHelper.SetUser(
            controller,
            _userId);

        return controller;
    }

    // ============================================================
    // Get User Voted Questions
    // ============================================================

    [Fact]
    public async Task GetUserVotedQuestions_SendsCorrectQuery()
    {
        // Arrange
        var response =
            ResponseFactory.Ok(
                Enumerable.Empty<VotedQuestionResponse>());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<GetUserVotedQuestionsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.GetUserVotedQuestions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<GetUserVotedQuestionsQuery>(
                    q => q.userId == _userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetUserVotedQuestions_ReturnsMediatorResponse()
    {
        // Arrange
        var response =
            ResponseFactory.Ok(
                Enumerable.Empty<VotedQuestionResponse>());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<GetUserVotedQuestionsQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.GetUserVotedQuestions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);
    }

    // ============================================================
    // Get User Voted Answers
    // ============================================================

    [Fact]
    public async Task GetUserVotedAnswers_SendsCorrectQuery()
    {
        // Arrange
        var response =
            ResponseFactory.Ok(
                Enumerable.Empty<VotedAnswerResponse>());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<GetUserVotedAnswersQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.GetUserVotedAnswers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<GetUserVotedAnswersQuery>(
                    q => q.userId == _userId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task GetUserVotedAnswers_ReturnsMediatorResponse()
    {
        // Arrange
        var response =
            ResponseFactory.Ok(
                Enumerable.Empty<VotedAnswerResponse>());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<GetUserVotedAnswersQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.GetUserVotedAnswers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);
    }

    // ============================================================
    // Vote Question
    // ============================================================

    [Fact]
    public async Task VoteQuestionAsync_SendsCorrectCommand()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var voteType = VoteType.Upvote;

        var response =
            ResponseFactory.Ok(
                new NewVoteResponse());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<VoteQuestionCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.VoteQuestionAsync(
            questionId,
            voteType);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<VoteQuestionCommand>(
                    command =>
                        command.QuestionId == questionId &&
                        command.UserId == _userId &&
                        command.Type == voteType),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task VoteQuestionAsync_SupportsDownvote()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var voteType = VoteType.Downvote;

        var response =
            ResponseFactory.Ok(
                new NewVoteResponse());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<VoteQuestionCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        await controller.VoteQuestionAsync(
            questionId,
            voteType);

        // Assert
        _mediatorMock.Verify(
            x => x.Send(
                It.Is<VoteQuestionCommand>(
                    command =>
                        command.QuestionId == questionId &&
                        command.UserId == _userId &&
                        command.Type == VoteType.Downvote),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // Vote Answer
    // ============================================================

    [Fact]
    public async Task VoteAnswerAsync_SendsCorrectCommand()
    {
        // Arrange
        var answerId = Guid.NewGuid();
        var voteType = VoteType.Upvote;

        var response =
            ResponseFactory.Ok(
                new NewVoteResponse());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<VoteAnswerCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.VoteAnswerAsync(
            answerId,
            voteType);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);
        _mediatorMock.Verify(
            x => x.Send(
                It.Is<VoteAnswerCommand>(
                    command =>
                        command.AnswerId == answerId &&
                        command.UserId == _userId &&
                        command.Type == voteType),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task VoteAnswerAsync_SupportsDownvote()
    {
        // Arrange
        var answerId = Guid.NewGuid();

        var response =
            ResponseFactory.Ok(
                new NewVoteResponse());

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<VoteAnswerCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        await controller.VoteAnswerAsync(
            answerId,
            VoteType.Downvote);

        // Assert
        _mediatorMock.Verify(
            x => x.Send(
                It.Is<VoteAnswerCommand>(
                    command =>
                        command.AnswerId == answerId &&
                        command.UserId == _userId &&
                        command.Type == VoteType.Downvote),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // ============================================================
    // Delete Vote
    // ============================================================

    [Fact]
    public async Task DeleteVoteAsync_SendsCorrectCommand()
    {
        // Arrange
        var voteId = Guid.NewGuid();

        var response =
            ResponseFactory.Ok(true);

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<DeleteVoteCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.DeleteVoteAsync(voteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<DeleteVoteCommand>(
                    command =>
                        command.UserId == _userId &&
                        command.voteId == voteId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteVoteAsync_ReturnsMediatorResponse()
    {
        // Arrange
        var voteId = Guid.NewGuid();

        var response =
            ResponseFactory.Ok(true);

        _mediatorMock
            .Setup(x => x.Send(
                It.IsAny<DeleteVoteCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = CreateController();

        // Act
        var result = await controller.DeleteVoteAsync(voteId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Same(response, okResult.Value);
    }
}