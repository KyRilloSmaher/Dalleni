using AutoMapper;
using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Application.Features.Categories.Queries.GetAll;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Infrasstructure.Handlers;
using Dalleni.UnitTests.Shared.Builders;
using MockQueryable.Moq;
using Moq;

namespace Dalleni.UnitTests.Modules.Categories.Queries;

public class GetAllCategoriesHandlerTests
{
    private readonly Mock<ICategoryRepository> _categories = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly IResponseHandler _responseHandler = new ResponseHandler();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public GetAllCategoriesHandlerTests()
    {
        _unitOfWork.Setup(u => u.Categories).Returns(_categories.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoriesExist_ReturnsMappedCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            CategoryTestData.Category("Programming"),
            CategoryTestData.Category("Databases")
        };

        var categoryDtos = new List<CategoryDto>
        {
            CategoryTestData.CategoryDto(name: "Programming"),
            CategoryTestData.CategoryDto(name: "Databases")
        };

        _categories
            .Setup(r => r.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        _mapper
            .Setup(m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()))
            .Returns(categoryDtos);

        var handler = new GetAllCategoriesHandler(_unitOfWork.Object, _responseHandler, _mapper.Object);

        // Act
        var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(SystemMessages.DATA_RETRIEVED, result.Message);
        Assert.Same(categoryDtos, result.Data);
        _categories.Verify(r => r.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
        _mapper.Verify(m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoCategoriesExist_ReturnsEmptyCollection()
    {
        // Arrange
        var emptyCategories = new List<Category>();
        var emptyDtos = new List<CategoryDto>();

        _categories
            .Setup(r => r.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyCategories);

        _mapper
            .Setup(m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()))
            .Returns(emptyDtos);

        var handler = new GetAllCategoriesHandler(_unitOfWork.Object, _responseHandler, _mapper.Object);

        // Act
        var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Empty(result.Data!);
        Assert.Equal(SystemMessages.DATA_RETRIEVED, result.Message);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrows_PropagatesException()
    {
        // Arrange
        _categories
            .Setup(r => r.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB failure"));

        var handler = new GetAllCategoriesHandler(_unitOfWork.Object, _responseHandler, _mapper.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None));

        _mapper.Verify(
            m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_PassesCancellationTokenToRepository()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        _categories
            .Setup(r => r.GetAllAsync(false, token))
            .ReturnsAsync(new List<Category>());

        _mapper
            .Setup(m => m.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()))
            .Returns(new List<CategoryDto>());

        var handler = new GetAllCategoriesHandler(_unitOfWork.Object, _responseHandler, _mapper.Object);

        // Act
        await handler.Handle(new GetAllCategoriesQuery(), token);

        // Assert
        _categories.Verify(r => r.GetAllAsync(false, token), Times.Once);
    }
}