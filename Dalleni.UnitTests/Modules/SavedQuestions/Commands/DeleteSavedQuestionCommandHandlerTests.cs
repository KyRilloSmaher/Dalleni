using Dalleni.Application.Features.SavedQuestions.Commands.DeleteSavedQuestion;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Moq;

namespace Dalleni.UnitTests.Modules.SavedQuestions.Commands;

public class DeleteSavedQuestionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();

    public DeleteSavedQuestionCommandHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.BadRequest<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<bool>(msg));
    }

    [Fact]
    public async Task Handle_ExistingSavedQuestion_DeletesAndReturnsSuccess()
    {
        var savedQuestion = EndpointTestData.SavedQuestion();
        var command = new DeleteSavedQuestionCommand(savedQuestion.Id);

        _unitOfWorkMock.Setup(x => x.SavedQuestionsRepository.GetByIdAsync(savedQuestion.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedQuestion);

        var handler = new DeleteSavedQuestionCommandHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        _unitOfWorkMock.Verify(x => x.SavedQuestionsRepository.Remove(savedQuestion), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NotFound_ReturnsBadRequest()
    {
        var command = new DeleteSavedQuestionCommand(Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.SavedQuestionsRepository.GetByIdAsync(command.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SavedQuestion?)null);

        var handler = new DeleteSavedQuestionCommandHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.FAILED, response.Message);
    }
}
