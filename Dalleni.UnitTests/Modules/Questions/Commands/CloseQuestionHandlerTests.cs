using Dalleni.Application.Features.Questions.Commands.CloseQuestion;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Questions.Commands;

public class CloseQuestionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<CloseQuestionHandler>> _loggerMock = new();

    public CloseQuestionHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<bool>(msg));
        _responseHandlerMock.Setup(x => x.Unauthorized<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.Unauthorized<bool>(msg));
    }

    [Fact]
    public async Task Handle_Author_ClosesQuestionAndReturnsSuccess()
    {
        var authorId = Guid.NewGuid();
        var question = EndpointTestData.Question(userId: authorId);
        var command = new CloseQuestionCommand(question.Id, authorId);

        _unitOfWorkMock.Setup(x => x.Questions.GetByIdAsync(question.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(question);

        var handler = new CloseQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.True(question.IsClosed);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
