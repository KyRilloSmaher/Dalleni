using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Application.Features.Rating.Queries.GetRatingById;
using Dalleni.Application.Features.Rating.Queries.GetRatingByUserId;
using Dalleni.Application.Features.Rating.Queries.GetRatingsByServiceIdQuery;
using Dalleni.Application.Features.Rating.Queries.GetUserRatingForService;
using Dalleni.Application.Features.Ratings.Commands.CreateRating;
using Dalleni.Application.Features.Ratings.Commands.DeleteRating;
using Dalleni.Application.Features.Ratings.Commands.UpdateRating;
using Dalleni.Domin.ResponsePattern;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.Ratings.Controllers;

public class RatingsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetByIdAsync_SendsGetRatingByIdQuery()
    {
        // Arrange
        var response = ResponseFactory.Ok(RatingTestData.RatingDto());
        _mediator.Setup(x => x.Send(It.IsAny<GetRatingByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);

        // Act
        var result = await controller.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<GetRatingByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByServiceIdAsync_SendsGetRatingsByServiceIdQuery()
    {
        // Arrange
        var response = ResponseFactory.Ok<IEnumerable<RatingDto>>(RatingTestData.RatingDtoList());
        _mediator.Setup(x => x.Send(It.IsAny<GetRatingsByServiceIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);

        // Act
        var result = await controller.GetByServiceIdAsync(Guid.NewGuid());

        // Assert
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.IsAny<GetRatingsByServiceIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMyRatingsAsync_SendsGetRatingsByUserIdQueryWithAuthenticatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var response = ResponseFactory.Ok<IEnumerable<RatingDto>>(RatingTestData.RatingDtoList());
        _mediator.Setup(x => x.Send(It.IsAny<GetRatingsByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.GetMyRatingsAsync();

        // Assert
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.Is<GetRatingsByUserIdQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetMyRatingForServiceAsync_SendsGetUserRatingForServiceQueryWithAuthenticatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var response = ResponseFactory.Ok(RatingTestData.RatingDto());
        _mediator.Setup(x => x.Send(It.IsAny<GetUserRatingForServiceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.GetMyRatingForServiceAsync(serviceId);

        // Assert
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.Is<GetUserRatingForServiceQuery>(q => 
            q.serviceId == serviceId && q.userId == userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_SendsCreateRatingCommandWithAuthenticatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateRatingRequestDto
        {
            ServiceId = Guid.NewGuid(),
            Value = 5,
            Comment = "Excellent service!",
            UserName = "Test User"
        };
        var response = ResponseFactory.Ok(RatingTestData.RatingDto());
        _mediator.Setup(x => x.Send(It.IsAny<CreateRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.CreateAsync(request);

        // Assert
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.Is<CreateRatingCommand>(c => 
            c.UserId == userId && c.Dto == request), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_SendsUpdateRatingCommandWithAuthenticatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ratingId = Guid.NewGuid();
        var request = new UpdateRatingRequestDto
        {
            Value = 4,
            Comment = "Updated comment"
        };
        var response = ResponseFactory.Ok(RatingTestData.RatingDto());
        _mediator.Setup(x => x.Send(It.IsAny<UpdateRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.UpdateAsync(ratingId, request);

        // Assert
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.Is<UpdateRatingCommand>(c => 
            c.userId == userId && c.Dto == request), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SendsDeleteRatingCommandWithAuthenticatedUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ratingId = Guid.NewGuid();
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.DeleteAsync(ratingId);

        // Assert
        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.Is<DeleteRatingCommand>(c => 
            c.rateId == ratingId && c.userId == userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenRatingNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var response = ResponseFactory.NotFound<RatingDto>("Rating not found");
        _mediator.Setup(x => x.Send(It.IsAny<GetRatingByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);

        // Act
        var result = await controller.GetByIdAsync(Guid.NewGuid());

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Same(response, notFoundResult.Value);
    }

    [Fact]
    public async Task CreateAsync_WhenUserAlreadyRated_ReturnsConflictResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateRatingRequestDto
        {
            ServiceId = Guid.NewGuid(),
            Value = 5,
            Comment = "Excellent service!"
        };
        var response = ResponseFactory.Conflict<RatingDto>("You have already rated this service");
        _mediator.Setup(x => x.Send(It.IsAny<CreateRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.CreateAsync(request);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Same(response, conflictResult.Value);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserIsNotOwner_ReturnsForbiddenResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ratingId = Guid.NewGuid();
        var request = new UpdateRatingRequestDto
        {
            Value = 4,
            Comment = "Updated comment"
        };
        var response = ResponseFactory.Forbidden<RatingDto>("You can only update your own ratings");
        _mediator.Setup(x => x.Send(It.IsAny<UpdateRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.UpdateAsync(ratingId, request);

        // Assert
        var forbiddenResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(403, forbiddenResult.StatusCode);
        Assert.Same(response, forbiddenResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_WhenRatingNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ratingId = Guid.NewGuid();
        var response = ResponseFactory.NotFound<bool>("Rating not found");
        _mediator.Setup(x => x.Send(It.IsAny<DeleteRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.DeleteAsync(ratingId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Same(response, notFoundResult.Value);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidRatingValue_ReturnsBadRequestResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateRatingRequestDto
        {
            ServiceId = Guid.NewGuid(),
            Value = 6, // Invalid value
            Comment = "Invalid rating"
        };
        var response = ResponseFactory.BadRequest<RatingDto>("Rating value must be between 1 and 5");
        _mediator.Setup(x => x.Send(It.IsAny<CreateRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.CreateAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Same(response, badRequestResult.Value);
    }

    [Fact]
    public async Task GetMyRatingsAsync_WhenNoUserInClaims_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var controller = new RatingsController(_mediator.Object);
        // Don't set user - this should throw

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => controller.GetMyRatingsAsync());
    }

    [Fact]
    public async Task GetMyRatingForServiceAsync_WhenNoUserInClaims_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var controller = new RatingsController(_mediator.Object);
        // Don't set user - this should throw

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => controller.GetMyRatingForServiceAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_WhenNoUserInClaims_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var controller = new RatingsController(_mediator.Object);
        var request = new CreateRatingRequestDto
        {
            ServiceId = Guid.NewGuid(),
            Value = 5,
            Comment = "Test"
        };
        // Don't set user - this should throw

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => controller.CreateAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_WhenNoUserInClaims_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var controller = new RatingsController(_mediator.Object);
        var request = new UpdateRatingRequestDto
        {
            Value = 4,
            Comment = "Updated"
        };
        // Don't set user - this should throw

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => controller.UpdateAsync(Guid.NewGuid(), request));
    }

    [Fact]
    public async Task DeleteAsync_WhenNoUserInClaims_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var controller = new RatingsController(_mediator.Object);
        // Don't set user - this should throw

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => controller.DeleteAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetMyRatingsAsync_ReturnsEmptyList_WhenUserHasNoRatings()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var response = ResponseFactory.Ok<IEnumerable<RatingDto>>(new List<RatingDto>());
        _mediator.Setup(x => x.Send(It.IsAny<GetRatingsByUserIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.GetMyRatingsAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Response<IEnumerable<RatingDto>>>(okResult.Value);
        Assert.Empty(returnValue.Data);
    }

    [Fact]
    public async Task GetByServiceIdAsync_ReturnsEmptyList_WhenServiceHasNoRatings()
    {
        // Arrange
        var response = ResponseFactory.Ok<IEnumerable<RatingDto>>(new List<RatingDto>());
        _mediator.Setup(x => x.Send(It.IsAny<GetRatingsByServiceIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);

        // Act
        var result = await controller.GetByServiceIdAsync(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Response<IEnumerable<RatingDto>>>(okResult.Value);
        Assert.Empty(returnValue.Data);
    }

    [Fact]
    public async Task GetMyRatingForServiceAsync_WhenUserHasNoRating_ReturnsNotFoundResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var serviceId = Guid.NewGuid();
        var response = ResponseFactory.NotFound<RatingDto>("Rating not found");
        _mediator.Setup(x => x.Send(It.IsAny<GetUserRatingForServiceQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.GetMyRatingForServiceAsync(serviceId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Same(response, notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteAsync_WhenUserIsNotOwner_ReturnsForbiddenResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ratingId = Guid.NewGuid();
        var response = ResponseFactory.Forbidden<bool>("You can only delete your own ratings");
        _mediator.Setup(x => x.Send(It.IsAny<DeleteRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.DeleteAsync(ratingId);

        // Assert
        var forbiddenResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(403, forbiddenResult.StatusCode);
        Assert.Same(response, forbiddenResult.Value);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsOkResultWithRating()
    {
        // Arrange
        var expectedRating = RatingTestData.RatingDto();
        var response = ResponseFactory.Ok(expectedRating);
        _mediator.Setup(x => x.Send(It.IsAny<GetRatingByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);

        // Act
        var result = await controller.GetByIdAsync(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Response<RatingDto>>(okResult.Value);
        Assert.Equal(expectedRating.Id, returnValue.Data.Id);
        Assert.Equal(expectedRating.Value, returnValue.Data.Value);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsOkResultWithCreatedRating()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateRatingRequestDto
        {
            ServiceId = Guid.NewGuid(),
            Value = 5,
            Comment = "Excellent service!"
        };
        var expectedRating = RatingTestData.RatingDto();
        var response = ResponseFactory.Ok(expectedRating, "Rating created successfully");
        _mediator.Setup(x => x.Send(It.IsAny<CreateRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.CreateAsync(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Response<RatingDto>>(okResult.Value);
        Assert.True(returnValue.Succeeded);
        Assert.Equal("Rating created successfully", returnValue.Message);
        Assert.Equal(expectedRating.Id, returnValue.Data.Id);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsOkResultWithUpdatedRating()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var ratingId = Guid.NewGuid();
        var request = new UpdateRatingRequestDto
        {
            Value = 4,
            Comment = "Updated comment"
        };
        var expectedRating = RatingTestData.RatingDto();
        expectedRating.Value = 4;
        expectedRating.Comment = "Updated comment";
        var response = ResponseFactory.Ok(expectedRating, "Rating updated successfully");
        _mediator.Setup(x => x.Send(It.IsAny<UpdateRatingCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new RatingsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, userId);

        // Act
        var result = await controller.UpdateAsync(ratingId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<Response<RatingDto>>(okResult.Value);
        Assert.True(returnValue.Succeeded);
        Assert.Equal("Rating updated successfully", returnValue.Message);
        Assert.Equal(4, returnValue.Data.Value);
        Assert.Equal("Updated comment", returnValue.Data.Comment);
    }
}



public static class RatingTestData
{
    public static RatingDto RatingDto(
        Guid? id = null,
        Guid? serviceId = null,
        Guid? userId = null,
        int value = 5,
        string comment = "Excellent service!",
        string userName = "Test User")
    {
        return new RatingDto
        {
            Id = id ?? Guid.NewGuid(),
            ServiceId = serviceId ?? Guid.NewGuid(),
            UserId = userId ?? Guid.NewGuid(),
            Value = value,
            Comment = comment,
            UserName = userName,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static List<RatingDto> RatingDtoList(int count = 3)
    {
        var list = new List<RatingDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(RatingDto(
                value: (i % 5) + 1,
                comment: $"Rating {i + 1}",
                userName: $"User{i + 1}"
            ));
        }
        return list;
    }

    public static List<RatingDto> RatingDtoListForService(Guid serviceId, int count = 3)
    {
        var list = new List<RatingDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(RatingDto(
                serviceId: serviceId,
                value: (i % 5) + 1,
                comment: $"Service rating {i + 1}",
                userName: $"User{i + 1}"
            ));
        }
        return list;
    }

    public static List<RatingDto> RatingDtoListForUser(Guid userId, int count = 3)
    {
        var list = new List<RatingDto>();
        for (int i = 0; i < count; i++)
        {
            list.Add(RatingDto(
                userId: userId,
                value: (i % 5) + 1,
                comment: $"User rating {i + 1}",
                userName: "Test User"
            ));
        }
        return list;
    }
}
