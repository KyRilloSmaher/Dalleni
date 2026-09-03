using Dalleni.Application.Features.Tags.Commands.CreateTag;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Tags.Commands;

public class CreateTagHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<CreateTagHandler>> _loggerMock = new();

    public CreateTagHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(It.IsAny<Guid>(), It.IsAny<string>()))
            .Returns((Guid id, string msg) => ResponseFactory.Ok(id, msg));
        _responseHandlerMock.Setup(x => x.BadRequest<Guid>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<Guid>(msg));
    }

    [Fact]
    public async Task Handle_UniqueName_CreatesTagAndReturnsSuccess()
    {
        var command = new CreateTagCommand("Architecture", "Software architecture tag");
        _unitOfWorkMock.Setup(x => x.Tags.ExistsByNormalizedNameAsync("architecture", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateTagHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        Assert.Equal(SystemMessages.RECORD_ADDED, response.Message);
        _unitOfWorkMock.Verify(x => x.Tags.AddAsync(It.IsAny<Tag>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateName_ReturnsBadRequest()
    {
        var command = new CreateTagCommand("Architecture", "Description");
        _unitOfWorkMock.Setup(x => x.Tags.ExistsByNormalizedNameAsync("architecture", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateTagHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.DUPLICATE_RECORD, response.Message);
    }
}
