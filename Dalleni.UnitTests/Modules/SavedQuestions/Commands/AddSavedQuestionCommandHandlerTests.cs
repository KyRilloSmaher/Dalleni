using Dalleni.Application.Features.SavedQuestions.Commands.AddSavedQuestion;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Responses;
using Moq;

namespace Dalleni.UnitTests.Modules.SavedQuestions.Commands;

public class AddSavedQuestionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();

    public AddSavedQuestionCommandHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(It.IsAny<SavedQuestion>(), It.IsAny<string>()))
            .Returns((SavedQuestion sq, string msg) => ResponseFactory.Ok(sq, msg));
        _responseHandlerMock.Setup(x => x.NotFound<SavedQuestion>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<SavedQuestion>(msg));
        _responseHandlerMock.Setup(x => x.BadRequest<SavedQuestion>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<SavedQuestion>(msg));
    }

    [Fact]
    public async Task Handle_ValidRequest_SavesQuestionAndReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var command = new AddSavedQuestionCommand(userId, questionId);

        _unitOfWorkMock.Setup(x => x.SavedQuestionsRepository.IsQuestionSavedByUserAsync(userId, questionId))
            .ReturnsAsync(false);
        _unitOfWorkMock.Setup(x => x.Questions.ExistsAsync(questionId))
            .ReturnsAsync(true);

        var handler = new AddSavedQuestionCommandHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.Equal(SystemMessages.SAVED_QUESTION_ADDED, response.Message);
        _unitOfWorkMock.Verify(x => x.SavedQuestionsRepository.AddAsync(It.IsAny<SavedQuestion>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_AlreadySaved_ReturnsBadRequest()
    {
        var userId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var command = new AddSavedQuestionCommand(userId, questionId);

        _unitOfWorkMock.Setup(x => x.SavedQuestionsRepository.IsQuestionSavedByUserAsync(userId, questionId))
            .ReturnsAsync(true);

        var handler = new AddSavedQuestionCommandHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.QUESTION_ALREADY_SAVED, response.Message);
    }

    [Fact]
    public async Task Handle_QuestionNotFound_ReturnsNotFound()
    {
        var userId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var command = new AddSavedQuestionCommand(userId, questionId);

        _unitOfWorkMock.Setup(x => x.SavedQuestionsRepository.IsQuestionSavedByUserAsync(userId, questionId))
            .ReturnsAsync(false);
        _unitOfWorkMock.Setup(x => x.Questions.ExistsAsync(questionId))
            .ReturnsAsync(false);

        var handler = new AddSavedQuestionCommandHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.NOT_FOUND, response.Message);
    }
}
