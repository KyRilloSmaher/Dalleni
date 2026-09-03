using Dalleni.Application.Features.Tags.Commands.MergeTags;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Tags.Commands;

public class MergeTagsHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();
    private readonly Mock<ILogger<MergeTagsHandler>> _loggerMock = new();

    public MergeTagsHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(true, It.IsAny<string>()))
            .Returns((bool b, string msg) => ResponseFactory.Ok(b, msg));
        _responseHandlerMock.Setup(x => x.NotFound<bool>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<bool>(msg));
    }

    [Fact]
    public async Task Handle_ValidTags_MergesSourceIntoTargetAndDeletesSource()
    {
        var sourceTag = EndpointTestData.Tag("source-tag");
        var targetTag = EndpointTestData.Tag("target-tag");
        var command = new MergeTagsCommand(sourceTag.Id, targetTag.Id);

        _unitOfWorkMock.Setup(x => x.Tags.GetByIdAsync(sourceTag.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceTag);
        _unitOfWorkMock.Setup(x => x.Tags.GetByIdAsync(targetTag.Id, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(targetTag);
        _unitOfWorkMock.Setup(x => x.QuestionTags.GetAllAsQueryableAsync())
            .ReturnsAsync(new List<QuestionTag>().AsQueryable());

        var handler = new MergeTagsHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        _unitOfWorkMock.Verify(x => x.Tags.Remove(sourceTag), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_MissingTag_ReturnsNotFound()
    {
        var command = new MergeTagsCommand(Guid.NewGuid(), Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Tags.GetByIdAsync(It.IsAny<Guid>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Tag?)null);

        var handler = new MergeTagsHandler(_unitOfWorkMock.Object, _responseHandlerMock.Object, _loggerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.RECORD_NOT_FOUND, response.Message);
    }
}
