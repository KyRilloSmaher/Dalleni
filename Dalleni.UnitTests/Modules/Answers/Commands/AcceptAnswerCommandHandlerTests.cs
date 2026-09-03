using Dalleni.Application.Features.Answers.Commands.AcceptAnswer;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Moq;

namespace Dalleni.UnitTests.Modules.Answers.Commands;

public class AcceptAnswerCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();

    public AcceptAnswerCommandHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<bool>(msg));
        _responseHandlerMock.Setup(x => x.BadRequest<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<bool>(msg));
    }

    [Fact]
    public async Task Handle_ValidRequest_AcceptsAnswerAndReturnsSuccess()
    {
        var authorId = Guid.NewGuid();
        var answerAuthorId = Guid.NewGuid();
        var answer = EndpointTestData.Answer(userId: answerAuthorId);
        var command = new AcceptAnswerCommand(answer.Id, authorId);

        _unitOfWorkMock.Setup(x => x.Answers.GetByIdAsync(answer.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var handler = new AcceptAnswerCommandHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.True(answer.IsAccepted);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_OwnAnswer_ReturnsBadRequest()
    {
        var authorId = Guid.NewGuid();
        var answer = EndpointTestData.Answer(userId: authorId);
        var command = new AcceptAnswerCommand(answer.Id, authorId);

        _unitOfWorkMock.Setup(x => x.Answers.GetByIdAsync(answer.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(answer);

        var handler = new AcceptAnswerCommandHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.CANNOT_UNACCEPT_OWN_ANSWER, response.Message);
    }
}
