using AutoMapper;
using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Application.Features.Ratings.Commands.CreateRating;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Responses;
using Moq;

namespace Dalleni.UnitTests.Modules.Ratings.Commands;

public class CreateRatingCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IResponseHandler> _responseHandlerMock = new();

    public CreateRatingCommandHandlerTests()
    {
        _responseHandlerMock.Setup(x => x.Success(It.IsAny<RatingDto>(), It.IsAny<string>()))
            .Returns((RatingDto dto, string msg) => ResponseFactory.Ok(dto, msg));
        _responseHandlerMock.Setup(x => x.NotFound<RatingDto>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.NotFound<RatingDto>(msg));
        _responseHandlerMock.Setup(x => x.BadRequest<RatingDto>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.BadRequest<RatingDto>(msg));
        _responseHandlerMock.Setup(x => x.Conflict<RatingDto>(It.IsAny<string>()))
            .Returns((string msg) => ResponseFactory.Conflict<RatingDto>(msg));
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesRatingAndReturnsSuccess()
    {
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new CreateRatingRequestDto { ServiceId = serviceId, Value = 5, Comment = "Great" };
        var command = new CreateRatingCommand(userId, dto);
        var ratingModel = EndpointTestData.Rating(serviceId, userId);
        var ratingDto = new RatingDto { Id = ratingModel.Id, Value = 5 };
        var service = Service.Create("Test service", "Service description", "Required documents", 0, Guid.NewGuid(), Guid.NewGuid());
        var user = EndpointTestData.User();

        _unitOfWorkMock.Setup(x => x.Services.GetByIdAsync(serviceId, true, CancellationToken.None))
            .ReturnsAsync(service);
        _unitOfWorkMock.Setup(x => x.Users.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.Ratings.GetUserRatingForServiceAsync(serviceId, userId))
            .ReturnsAsync((Rating?)null);
        _mapperMock.Setup(x => x.Map<Rating>(command)).Returns(ratingModel);
        _mapperMock.Setup(x => x.Map<RatingDto>(ratingModel)).Returns(ratingDto);

        var handler = new CreateRatingCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.True(response.Succeeded);
        _unitOfWorkMock.Verify(x => x.Ratings.AddAsync(ratingModel, CancellationToken.None), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ServiceNotFound_ReturnsNotFound()
    {
        var serviceId = Guid.NewGuid();
        var dto = new CreateRatingRequestDto { ServiceId = serviceId, Value = 5 };
        var command = new CreateRatingCommand(Guid.NewGuid(), dto);

        _unitOfWorkMock.Setup(x => x.Services.GetByIdAsync(serviceId, true, CancellationToken.None))
            .ReturnsAsync((Service?)null);

        var handler = new CreateRatingCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.SERVICE_NOT_FOUND, response.Message);
    }

    [Fact]
    public async Task Handle_AlreadyRated_ReturnsBadRequest()
    {
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new CreateRatingRequestDto { ServiceId = serviceId, Value = 4 };
        var command = new CreateRatingCommand(userId, dto);
        var existingRating = EndpointTestData.Rating(serviceId, userId);
        var service = Service.Create("Test service", "Service description", "Required documents", 0, Guid.NewGuid(), Guid.NewGuid());
        var user = EndpointTestData.User();

        _unitOfWorkMock.Setup(x => x.Services.GetByIdAsync(serviceId, true, CancellationToken.None))
            .ReturnsAsync(service);
        _unitOfWorkMock.Setup(x => x.Users.GetByIdAsync(userId))
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.Ratings.GetUserRatingForServiceAsync(serviceId, userId))
            .ReturnsAsync(existingRating);

        var handler = new CreateRatingCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _responseHandlerMock.Object);

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.False(response.Succeeded);
        Assert.Equal(SystemMessages.AlREADY_RATED, response.Message);
    }
}
