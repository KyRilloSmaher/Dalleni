
using AutoMapper;
using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Application.Features.Votes.Queries.GetUserVotedAnswersQuery;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Votes.Queries;

public class GetUserVotedAnswersQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<GetUserVotedAnswersQueryHandler>> _loggerMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    public GetUserVotedAnswersQueryHandlerTests()
    {
        _responseHandlerMock
            .Setup(x => x.BadRequest<IEnumerable<VotedAnswerResponse>>(
                It.IsAny<string>()))
            .Returns((string message) =>
                ResponseFactory.BadRequest<IEnumerable<VotedAnswerResponse>>(message));

        _responseHandlerMock
            .Setup(x => x.Success(
                It.IsAny<IEnumerable<VotedAnswerResponse>>(),
                It.IsAny<string>()))
            .Returns((
                IEnumerable<VotedAnswerResponse> data,
                string message) =>
                ResponseFactory.Ok(data, message));
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _unitOfWorkMock
            .Setup(x => x.Users.GetByIdAsync(userId, false))
            .ReturnsAsync((ApplicationUser?)null);

        var query = new GetUserVotedAnswersQuery(userId);

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);
        Assert.Equal(
            SystemMessages.USER_NOT_FOUND,
            response.Message);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetAllUserVotesAsync(
                It.IsAny<Guid>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _mapperMock.Verify(
            x => x.Map<IEnumerable<VotedAnswerResponse>>(
                It.IsAny<object>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UserExistsButVotesNotFound_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new Mock<ApplicationUser>().Object;

        _unitOfWorkMock
            .Setup(x => x.Users.GetByIdAsync(userId, false))
            .ReturnsAsync(user);

        _unitOfWorkMock
            .Setup(x => x.Votes.GetAllUserVotesAsync(
                userId,
                false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Vote>?)null);

        var query = new GetUserVotedAnswersQuery(userId);

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.False(response.Succeeded);
        Assert.Equal(
            SystemMessages.NOT_FOUND,
            response.Message);

        _mapperMock.Verify(
            x => x.Map<IEnumerable<VotedAnswerResponse>>(
                It.IsAny<object>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UserHasVotedAnswers_ReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new Mock<ApplicationUser>().Object;

        var votes = new List<Vote>
        {
            new Mock<Vote>().Object,
            new Mock<Vote>().Object
        };
       var dtos = new List<VotedAnswerResponse>
        {
            new()
            {
                VoteId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                VoteId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            }
        };

        _unitOfWorkMock
            .Setup(x => x.Users.GetByIdAsync(userId, false))
            .ReturnsAsync(user);

        _unitOfWorkMock
            .Setup(x => x.Votes.GetAllUserVotesAsync(
                userId,
                false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(votes);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<VotedAnswerResponse>>(votes))
            .Returns(dtos);

        var query = new GetUserVotedAnswersQuery(userId);

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);
        Assert.Equal(
            SystemMessages.DATA_RETRIEVED,
            response.Message);

        Assert.Same(dtos, response.Data);

        _unitOfWorkMock.Verify(
            x => x.Votes.GetAllUserVotesAsync(
                userId,
                false,
                It.IsAny<CancellationToken>()),
            Times.Once);

        _mapperMock.Verify(
            x => x.Map<IEnumerable<VotedAnswerResponse>>(votes),
            Times.Once);
    }

    [Fact]
    public async Task Handle_UserHasNoVotedAnswers_ReturnsSuccessWithEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new Mock<ApplicationUser>().Object;

        var votes = Enumerable.Empty<Vote>();
        var dtos = Enumerable.Empty<VotedAnswerResponse>();

        _unitOfWorkMock
            .Setup(x => x.Users.GetByIdAsync(userId, false))
            .ReturnsAsync(user);

        _unitOfWorkMock
            .Setup(x => x.Votes.GetAllUserVotesAsync(
                userId,
                false,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(votes);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<VotedAnswerResponse>>(votes))
            .Returns(dtos);

        var query = new GetUserVotedAnswersQuery(userId);

        var handler = CreateHandler();

        // Act
        var response = await handler.Handle(
            query,
            CancellationToken.None);

        // Assert
        Assert.True(response.Succeeded);
        Assert.Equal(
            SystemMessages.DATA_RETRIEVED,
            response.Message);

        Assert.Empty(response.Data!);

        _mapperMock.Verify(
            x => x.Map<IEnumerable<VotedAnswerResponse>>(votes),
            Times.Once);
    }

    [Fact]
    public async Task Handle_PassesCorrectUserIdAndCancellationToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new Mock<ApplicationUser>().Object;
        var cancellationToken =
            new CancellationTokenSource().Token;

        var votes = new List<Vote>();
        var dtos = Enumerable.Empty<VotedAnswerResponse>();

        _unitOfWorkMock
            .Setup(x => x.Users.GetByIdAsync(userId, false))
            .ReturnsAsync(user);

        _unitOfWorkMock
            .Setup(x => x.Votes.GetAllUserVotesAsync(
                userId,
                false,
                cancellationToken))
            .ReturnsAsync(votes);

        _mapperMock
            .Setup(x => x.Map<IEnumerable<VotedAnswerResponse>>(votes))
            .Returns(dtos);

        var query = new GetUserVotedAnswersQuery(userId);

        var handler = CreateHandler();

        // Act
        await handler.Handle(query, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(
            x => x.Votes.GetAllUserVotesAsync(
                userId,
                false,
                cancellationToken),
            Times.Once);
    }
    private GetUserVotedAnswersQueryHandler CreateHandler()
    {
        return new GetUserVotedAnswersQueryHandler(
            _unitOfWorkMock.Object,
            _responseHandlerMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }
}