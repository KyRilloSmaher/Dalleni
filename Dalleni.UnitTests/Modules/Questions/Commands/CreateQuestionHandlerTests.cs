using Dalleni.Application.DTOs.Requests.Questions;
using Dalleni.Application.Features.Questions.Commands.CreateQuestion;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Questions.Commands;

public class CreateQuestionHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<CreateQuestionHandler>> _loggerMock = new();

    public CreateQuestionHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns((Guid id, string msg) => ResponseFactory.Ok(id, msg));
        _responseHandlerMock.Setup(x => x.NotFound<Guid>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<Guid>(msg));
        _responseHandlerMock.Setup(x => x.BadRequest<Guid>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<Guid>(msg));
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesQuestionAndReturnsSuccess()
    {
        var user = EndpointTestData.User();
        user.Id = Guid.NewGuid(); // Ensure the user has a valid ID
        var dto = new CreateQuestionRequestDto
        {
            Title = "How to unit test MediatR?",
            Content = "Looking for best practices in C# xUnit.",
            CategoryId = Guid.NewGuid(),
            Tags = new List<string> { "c#", "dotnet", "xunit" }
        };
        var command = new CreateQuestionCommand(dto, user.Id);

        _unitOfWorkMock.Setup(x => x.UserManager.FindByIdAsync(user.Id, true))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.Tags.GetByNormalizedNamesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Tag>());
        _unitOfWorkMock.Setup(x => x.Tags.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(x => x.Questions.AddAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded, response.Message);
        Assert.Equal(SystemMessages.RECORD_ADDED, response.Message);
        _unitOfWorkMock.Verify(x => x.Questions.AddAsync(It.IsAny<Question>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNotFound()
    {
        var dto = new CreateQuestionRequestDto
        {
            Title = "Title",
            Content = "Content",
            CategoryId = Guid.NewGuid()
        };
        var command = new CreateQuestionCommand(dto, Guid.NewGuid());

        _unitOfWorkMock.Setup(x => x.UserManager.FindByIdAsync(command.UserId, true))
            .ReturnsAsync((ApplicationUser?)null);

        var handler = new CreateQuestionHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.USER_NOT_FOUND, response.Message);
    }
}
