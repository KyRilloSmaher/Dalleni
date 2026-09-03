using Dalleni.Application.Features.Questions.Commands.DeleteQuestion;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Questions.Commands;

public class DeleteQuestionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<DeleteQuestionHandler>> _loggerMock = new();

    public DeleteQuestionHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<bool>(msg));
        _responseHandlerMock.Setup(x => x.Unauthorized<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.Unauthorized<bool>(msg));
    }

    [Fact]
    public async Task Handle_Author_RemovesQuestionAndReturnsSuccess()
    {
        var authorId = Guid.NewGuid();
        var question = EndpointTestData.Question(userId: authorId);
        var command = new DeleteQuestionCommand(question.Id, authorId);

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var handler = new DeleteQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        _unitOfWorkMock.Verify(x => x.Questions.Remove(question), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NotFound_ReturnsNotFound()
    {
        var command = new DeleteQuestionCommand(Guid.NewGuid(), Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(command.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Question?)null);

        var handler = new DeleteQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.RECORD_NOT_FOUND, response.Message);
    }

    [Fact]
    public async Task Handle_Unauthorized_ReturnsUnauthorized()
    {
        var question = EndpointTestData.Question(userId: Guid.NewGuid());
        var command = new DeleteQuestionCommand(question.Id, Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var handler = new DeleteQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.ACCESS_DENIED, response.Message);
    }
}
