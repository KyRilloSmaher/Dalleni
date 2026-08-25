using AutoMapper;
using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Application.Features.Categories.Queries;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Infrasstructure.Handlers;
using Dalleni.UnitTests.Shared.Builders;
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
        _unitOfWork.Setup(unitOfWork => unitOfWork.Categories).Returns(_categories.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoriesExist_ReturnsMappedCategories()
    {
        var categories = new[]
        {
            CategoryTestData.Category("Programming"),
            CategoryTestData.Category("Databases")
        };
        var categoryDtos = new[]
        {
            CategoryTestData.CategoryDto(name: "Programming"),
            CategoryTestData.CategoryDto(name: "Databases")
        };

        _categories
            .Setup(repository => repository.GetAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        _mapper
            .Setup(mapper => mapper.Map<IEnumerable<CategoryDto>>(It.IsAny<IEnumerable<Category>>()))
            .Returns(categoryDtos);

        var handler = new GetAllCategoriesHandler(_unitOfWork.Object, _responseHandler, _mapper.Object);

        var result = await handler.Handle(new GetAllCategoriesQuery(), CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal(SystemMessages.DATA_RETRIEVED, result.Message);
        Assert.Same(categoryDtos, result.Data);
        _categories.Verify(repository => repository.GetAllAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }
}
