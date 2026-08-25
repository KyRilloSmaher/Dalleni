using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Application.Features.Tags.Queries.GetTopTags;
using Dalleni.Domin.ResponsePattern;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.Tags.Controllers;

public class TagsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetAllAsync_SendsGetTopTagsQueryAndReturnsOk()
    {
        var response = ResponseFactory.Ok<PaginatedResult<TagDto>>(EndpointTestData.PagedTags());
        _mediator.Setup(x => x.Send(It.IsAny<GetTopTagsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new TagsController(_mediator.Object);

        var result = await controller.GetAllAsync(new PagedRequest());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<GetTopTagsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

