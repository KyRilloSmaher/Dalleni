using Dalleni.Application.DTOs.Requests.Answers;
using Dalleni.Application.Features.Answers.Commands.UpdateAnswer;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Answers.Commands;

public class UpdateAnswerHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<UpdateAnswerHandler>> _loggerMock = new();

    public UpdateAnswerHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<bool>(msg));
        _responseHandlerMock.Setup(x => x.Unauthorized<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.Unauthorized<bool>(msg));
    }

    [Fact]
    public async Task Handle_ValidAuthor_UpdatesAnswerAndReturnsSuccess()
    {
        var authorId = Guid.NewGuid();
        var answer = EndpointTestData.Answer(userId: authorId);
        var dto = new UpdateAnswerRequestDto { Content = "Updated content text" };
        var command = new UpdateAnswerCommand(answer.Id, dto, authorId);

        _unitOfWorkMock.Setup(x => x.Answers.GetByIdAsync(answer.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var handler = new UpdateAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.Equal("Updated content text", answer.Content);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AnswerNotFound_ReturnsNotFound()
    {
        var command = new UpdateAnswerCommand(Guid.NewGuid(), new UpdateAnswerRequestDto(), Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Answers.GetByIdAsync(command.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Answer?)null);

        var handler = new UpdateAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.RECORD_NOT_FOUND, response.Message);
    }

    [Fact]
    public async Task Handle_UnauthorizedUser_ReturnsUnauthorized()
    {
        var answer = EndpointTestData.Answer(userId: Guid.NewGuid());
        var command = new UpdateAnswerCommand(answer.Id, new UpdateAnswerRequestDto(), Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.Answers.GetByIdAsync(answer.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var handler = new UpdateAnswerHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.ACCESS_DENIED, response.Message);
    }
}
