using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Application.Features.Categories.Queries;
using Dalleni.Domin.Helpers;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.Categories.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetAllAsync_WhenMediatorReturnsSuccess_ReturnsOkWithCategories()
    {
        var categories = new[]
        {
            CategoryTestData.CategoryDto(name: "Programming"),
            CategoryTestData.CategoryDto(name: "Databases")
        };
        var response = ResponseFactory.Ok<IEnumerable<CategoryDto>>(categories, SystemMessages.DATA_RETRIEVED);

        _mediator
            .Setup(mediator => mediator.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var controller = new CategoriesController(_mediator.Object);

        var result = await controller.GetAllAsync();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(response, okResult.Value);
        _mediator.Verify(
            mediator => mediator.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
