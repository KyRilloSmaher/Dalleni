using Dalleni.Application.DTOs.Requests.Answers;
using Dalleni.Application.Features.Answers.Commands.CreateAnswer;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Answers.Commands;

public class CreateAnswerHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<CreateAnswerHandler>> _loggerMock = new();

    public CreateAnswerHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns((Guid id, string msg) => ResponseFactory.Ok(id, msg));
        _responseHandlerMock.Setup(x => x.NotFound<Guid>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<Guid>(msg));
        _responseHandlerMock.Setup(x => x.BadRequest<Guid>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<Guid>(msg));
    }
[Fact]
public async Task Handle_ValidRequest_CreatesAnswerAndReturnsSuccess()
{
    // Arrange
    var question = EndpointTestData.Question();
    var user = EndpointTestData.User();

    user.Id = Guid.NewGuid();

    var dto = new CreateAnswerRequestDto
    {
        QuestionId = question.Id,
        Content = "Great answer content"
    };

    var command = new CreateAnswerCommand(dto, user.Id);

    _unitOfWorkMock
        .Setup(x => x.Questions.GetByIdAsync(
            question.Id,
            true,
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(question);

    _unitOfWorkMock
        .Setup(x => x.Users.GetByIdAsync(
            user.Id,
            true))
        .ReturnsAsync(user);

    _unitOfWorkMock
        .Setup(x => x.Answers.AddAsync(
            It.IsAny<Answer>(),
            It.IsAny<CancellationToken>()))
        .Returns(Task.CompletedTask);

    _unitOfWorkMock
        .Setup(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()))
        .ReturnsAsync(1);

    var handler = new CreateAnswerHandler(
        _unitOfWorkMock.Object,
        _responseHandlerMock.Object,
        _loggerMock.Object);

    // Act
    var response = await handler.Handle(
        command,
        CancellationToken.None);

    // Assert
    Assert.True(response.Succeeded, response.Message);
    Assert.Equal(
        SystemMessages.RECORD_ADDED,
        response.Message);

    _unitOfWorkMock.Verify(
        x => x.Users.GetByIdAsync(
            user.Id,
            true),
        Times.Once);

    _unitOfWorkMock.Verify(
        x => x.Answers.AddAsync(
            It.IsAny<Answer>(),
            It.IsAny<CancellationToken>()),
        Times.Once);

    _unitOfWorkMock.Verify(
        x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
        Times.Once);
}
    [Fact]
    public async Task Handle_QuestionNotFound_ReturnsNotFound()
    {
        var dto = new CreateAnswerRequestDto { QuestionId = Guid.NewGuid(), Content = "Answer text" };
        var command = new CreateAnswerCommand(dto, Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(dto.QuestionId, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Question?)null);

        var handler = new CreateAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.RECORD_NOT_FOUND, response.Message);
    }

    [Fact]
    public async Task Handle_QuestionIsClosed_ReturnsBadRequest()
    {
        var question = EndpointTestData.Question();
        question.Close();
        var user = EndpointTestData.User();
        var dto = new CreateAnswerRequestDto { QuestionId = question.Id, Content = "Answer text" };
        var command = new CreateAnswerCommand(dto, user.Id);

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var handler = new CreateAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal("Cannot answer a closed question.", response.Message);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        var question = EndpointTestData.Question();
        var dto = new CreateAnswerRequestDto { QuestionId = question.Id, Content = "Answer text" };
        var command = new CreateAnswerCommand(dto, Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);
        _unitOfWorkMock.Setup(x => x.Users.GetByIdAsync(command.UserId, true))
            .ReturnsAsync((ApplicationUser?)null);

        var handler = new CreateAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.USER_NOT_FOUND, response.Message);
    }
}
