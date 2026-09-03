using Dalleni.Application.DTOs.Requests.Questions;
using Dalleni.Application.Features.Questions.Commands.UpdateQuestion;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Questions.Commands;

public class UpdateQuestionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<UpdateQuestionHandler>> _loggerMock = new();

    public UpdateQuestionHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<bool>(msg));
        _responseHandlerMock.Setup(x => x.Unauthorized<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.Unauthorized<bool>(msg));
    }

    [Fact]
    public async Task Handle_Author_UpdatesQuestionAndReturnsSuccess()
    {
        var authorId = Guid.NewGuid();
        var question = EndpointTestData.Question(userId: authorId);
        var dto = new UpdateQuestionRequestDto { Title = "New Title", Content = "New Content", CategoryId = Guid.NewGuid() };
        var command = new UpdateQuestionCommand(question.Id, dto, authorId);

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var handler = new UpdateQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.Equal("New Title", question.Title);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NotFound_ReturnsNotFound()
    {
        var command = new UpdateQuestionCommand(Guid.NewGuid(), new UpdateQuestionRequestDto(), Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(command.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Question?)null);

        var handler = new UpdateQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.RECORD_NOT_FOUND, response.Message);
    }

    [Fact]
    public async Task Handle_Unauthorized_ReturnsUnauthorized()
    {
        var question = EndpointTestData.Question(userId: Guid.NewGuid());
        var command = new UpdateQuestionCommand(question.Id, new UpdateQuestionRequestDto(), Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var handler = new UpdateQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.ACCESS_DENIED, response.Message);
    }
}
