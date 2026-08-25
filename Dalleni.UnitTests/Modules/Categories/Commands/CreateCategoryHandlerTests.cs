using Dalleni.Application.Features.Categories.Commands.CreateCategory;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Infrasstructure.Handlers;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dalleni.UnitTests.Modules.Categories.Commands;

public class CreateCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categories = new();
    private readonly Mock<ILogger<CreateCategoryHandler>> _logger = new();
    private readonly IResponseHandler _responseHandler = new ResponseHandler();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public CreateCategoryHandlerTests()
    {
        _unitOfWork.Setup(unitOfWork => unitOfWork.Categories).Returns(_categories.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryIsValid_AddsCategoryAndSavesChanges()
    {
        Category? addedCategory = null;

        _categories
            .Setup(repository => repository.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((category, _) => addedCategory = category)
            .Returns(Task.CompletedTask);

        _unitOfWork
            .Setup(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateCategoryHandler(_unitOfWork.Object, _responseHandler, _logger.Object);

        var result = await handler.Handle(new CreateCategoryCommand("Programming", "Code questions"), CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal(SystemMessages.RECORD_ADDED, result.Message);
        Assert.NotEqual(Guid.Empty, result.Data);
        Assert.NotNull(addedCategory);
        Assert.Equal("Programming", addedCategory!.Name);
        _categories.Verify(repository => repository.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCategoryCannotBeCreated_ReturnsBadRequest()
    {
        var handler = new CreateCategoryHandler(_unitOfWork.Object, _responseHandler, _logger.Object);

        var result = await handler.Handle(new CreateCategoryCommand("", null), CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        _categories.Verify(repository => repository.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(unitOfWork => unitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
