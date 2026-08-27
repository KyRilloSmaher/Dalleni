using Dalleni.API.Controllers;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Requests.Questions;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.Features.Questions.Commands.AcceptAnswer;
using Dalleni.Application.Features.Questions.Commands.CloseQuestion;
using Dalleni.Application.Features.Questions.Commands.CreateQuestion;
using Dalleni.Application.Features.Questions.Commands.DeleteQuestion;
using Dalleni.Application.Features.Questions.Commands.UpdateQuestion;
using Dalleni.Application.Features.Questions.Queries.GetByTag;
using Dalleni.Application.Features.Questions.Queries.GetPagedQuestions;
using Dalleni.Application.Features.Questions.Queries.GetQuestionDetails;
using Dalleni.Application.Features.Questions.Queries.GetRelatedQuestions;
using Dalleni.Application.Features.Questions.Queries.GetSimilars;
using Dalleni.Application.Features.Questions.Queries.Search;
using Dalleni.Domin.ResponsePattern;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Controllers;
using Dalleni.UnitTests.Shared.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Dalleni.UnitTests.Modules.Questions.Controllers;

public class QuestionsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();

    [Fact]
    public async Task GetByIdAsync_SendsDetailsQuery()
    {
        var response = ResponseFactory.Ok(EndpointTestData.QuestionDetails());
        _mediator.Setup(x => x.Send(It.IsAny<GetQuestionDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);

        var result = await controller.GetByIdAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
public async Task GetAllPagedAsync_SendsPagedQuery()
{
    var response = ResponseFactory.Ok<PaginatedResult<QuestionDetailsResponseDto>>(EndpointTestData.PagedQuestions());
    
    // ✅ Use IRequest<Response<PaginatedResult<QuestionDetailsResponseDto>>>
    _mediator.Setup(x => x.Send(
        It.IsAny<IRequest<Response<PaginatedResult<QuestionDetailsResponseDto>>>>(), 
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(response);
    
    var controller = new QuestionsController(_mediator.Object);

    var result = await controller.GetAllPagedAsync(new PagedRequest());

    Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
}

[Fact]
public async Task GetByTagAsync_SendsGetByTagQuery()
{
    var response = ResponseFactory.Ok<PaginatedResult<QuestionDetailsResponseDto>>(EndpointTestData.PagedQuestions());
    
    // ✅ Use IRequest<Response<PaginatedResult<QuestionDetailsResponseDto>>>
    _mediator.Setup(x => x.Send(
        It.IsAny<IRequest<Response<PaginatedResult<QuestionDetailsResponseDto>>>>(), 
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(response);
    
    var controller = new QuestionsController(_mediator.Object);

    var result = await controller.GetByTagAsync(new PagedRequest(), Guid.NewGuid());

    Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
}

[Fact]
public async Task SearchAsync_SendsSearchQuery()
{
    var response = ResponseFactory.Ok<PaginatedResult<QuestionDetailsResponseDto>>(EndpointTestData.PagedQuestions());
    
    // ✅ Use IRequest<Response<PaginatedResult<QuestionDetailsResponseDto>>>
    _mediator.Setup(x => x.Send(
        It.IsAny<IRequest<Response<PaginatedResult<QuestionDetailsResponseDto>>>>(), 
        It.IsAny<CancellationToken>()))
        .ReturnsAsync(response);
    
    var controller = new QuestionsController(_mediator.Object);

    var result = await controller.SearchAsync(new PagedRequest(), "dotnet");

    Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
}

    [Fact]
    public async Task RelatedQuestionsAsync_WhenCountIsInvalid_UsesDefaultCount()
    {
        var response = ResponseFactory.Ok<IEnumerable<QuestionDetailsResponseDto>>(new[] { EndpointTestData.QuestionDetails() });
        _mediator.Setup(x => x.Send(It.IsAny<GetRelatedQuestionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);

        var result = await controller.RelatedQuestionsAsync(Guid.NewGuid(), 0);

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
        _mediator.Verify(x => x.Send(It.Is<GetRelatedQuestionsQuery>(query => query.Count == 5), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SimilarsQuestionaAsync_SendsSimilarQuestionsQuery()
    {
        var response = ResponseFactory.Ok<IEnumerable<QuestionDetailsResponseDto>>(new[] { EndpointTestData.QuestionDetails() });
        _mediator.Setup(x => x.Send(It.IsAny<SimilarQuestionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);

        var result = await controller.SimilarsQuestionaAsync("How to test?");

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task CloseAsync_SendsCloseCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<CloseQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.CloseAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task AcceptAnswerAsync_SendsAcceptAnswerCommand()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<AcceptAnswerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.AcceptAnswerAsync(Guid.NewGuid(), Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task CreateAsync_SendsCreateCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(Guid.NewGuid());
        _mediator.Setup(x => x.Send(It.IsAny<CreateQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.CreateAsync(new CreateQuestionRequestDto
        {
            Title = "Title",
            Content = "Content",
            CategoryId = Guid.NewGuid(),
            Tags = new List<string> { "dotnet" }
        });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task UpdateAsync_SendsUpdateCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<UpdateQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.UpdateAsync(Guid.NewGuid(), new UpdateQuestionRequestDto
        {
            Title = "Title",
            Content = "Content",
            CategoryId = Guid.NewGuid()
        });

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }

    [Fact]
    public async Task DeleteAsync_SendsDeleteCommandWithAuthenticatedUser()
    {
        var response = ResponseFactory.Ok(true);
        _mediator.Setup(x => x.Send(It.IsAny<DeleteQuestionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var controller = new QuestionsController(_mediator.Object);
        ControllerTestHelper.SetUser(controller, Guid.NewGuid());

        var result = await controller.DeleteAsync(Guid.NewGuid());

        Assert.Same(response, Assert.IsType<OkObjectResult>(result).Value);
    }
}

