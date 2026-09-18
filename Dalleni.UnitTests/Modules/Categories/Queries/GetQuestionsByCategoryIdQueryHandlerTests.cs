using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.Features.Categories.Queries.GetQuestionsByCategoryId;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Infrasstructure.Handlers;
using Dalleni.UnitTests.Shared.Builders;
using MockQueryable;
using MockQueryable.Moq;
using Moq;

namespace Dalleni.UnitTests.Modules.Categories.Queries;

public class GetQuestionsByCategoryIdQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categories = new();
    private readonly Mock<IQuestionRepository> _questions = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly IResponseHandler _responseHandler = new ResponseHandler();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    public GetQuestionsByCategoryIdQueryHandlerTests()
    {
        _unitOfWork.Setup(u => u.Categories).Returns(_categories.Object);
        _unitOfWork.Setup(u => u.Questions).Returns(_questions.Object);
    }

    private GetQuestionsByCategoryIdQueryHandler CreateHandler() =>
        new(_unitOfWork.Object, _responseHandler, _mapper.Object);


    private static GetQuestionsByCategoryIdQuery BuildQuery(Guid categoryId, int pageNumber = 1, int pageSize = 10) =>
        new(new PagedRequest { PageNumber = pageNumber, PageSize = pageSize }, categoryId);

    [Fact]
    public async Task Handle_WhenCategoryNotFound_ReturnsNotFound()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _categories
            .Setup(r => r.ExistsAsync(categoryId))
            .ReturnsAsync(false);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(BuildQuery(categoryId), CancellationToken.None);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(SystemMessages.CATEGORY_NOT_FOUND, result.Message);

        _questions.Verify(
            r => r.GetByCategoryIdAsync(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_ReturnsPaginatedQuestions()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var questions = new List<Question>
        {
            QuestionTestData.Question("Q1", categoryId: categoryId),
            QuestionTestData.Question("Q2", categoryId: categoryId),
            QuestionTestData.Question("Q3", categoryId: categoryId),
        };

        // EF-friendly async-capable IQueryable
        var mockQuestions = questions.BuildMock();

        var mockDtos = questions
            .Select(q => new QuestionSummaryDto { Id = q.Id, Title = q.Title })
            .ToList()
            .BuildMock();

        _categories
            .Setup(r => r.ExistsAsync(categoryId))
            .ReturnsAsync(true);

        _questions
            .Setup(r => r.GetByCategoryIdAsync(categoryId))
            .ReturnsAsync(mockQuestions);

        _mapper
            .Setup(m => m.ProjectTo<QuestionSummaryDto>(
                It.IsAny<IQueryable<Question>>(),
                It.IsAny<object>()))
            .Returns(mockDtos);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(BuildQuery(categoryId, pageNumber: 1, pageSize: 2), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(SystemMessages.DATA_RETRIEVED, result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Items.Count());
        Assert.Equal(3, result.Data.TotalCount);
        _categories.Verify(r => r.ExistsAsync(categoryId), Times.Once);
        _questions.Verify(r => r.GetByCategoryIdAsync(categoryId), Times.Once);
    }
    [Fact]
    public async Task  Handle_WhenRepositoryReturnsOnlyMatchingQuestions_ReturnsFilteredPaginatedResult()
    {

        // Arrange
        var categoryId = Guid.NewGuid();

        var questions = new List<Question>
        {
            QuestionTestData.Question("Q1", categoryId: categoryId),
            QuestionTestData.Question("Q2", categoryId: categoryId),
            QuestionTestData.Question("Q3", categoryId: categoryId),
            QuestionTestData.Question("Q4", categoryId: Guid.NewGuid()), // Different category
            QuestionTestData.Question("Q5", categoryId: Guid.NewGuid()), // Different category
        };

        // EF-friendly async-capable IQueryable
        var filteredQuery = questions
        .BuildMock()
        .Where(q => q.CategoryId == categoryId);

        var mockDtos = filteredQuery
            .Select(q => new QuestionSummaryDto { Id = q.Id, Title = q.Title })
            .ToList()
            .BuildMock();

        _categories
            .Setup(r => r.ExistsAsync(categoryId))
            .ReturnsAsync(true);

        _questions
            .Setup(r => r.GetByCategoryIdAsync(categoryId))
            .ReturnsAsync(filteredQuery);

        _mapper
            .Setup(m => m.ProjectTo<QuestionSummaryDto>(
                It.IsAny<IQueryable<Question>>(),
                It.IsAny<object>()))
            .Returns(mockDtos);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(BuildQuery(categoryId, pageNumber: 1, pageSize: 5), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(SystemMessages.DATA_RETRIEVED, result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(3, result.Data.Items.Count());
        Assert.Equal(3, result.Data.TotalCount); 
        Assert.All(result.Data.Items, dto => Assert.StartsWith("Q", dto.Title));
        _categories.Verify(r => r.ExistsAsync(categoryId), Times.Once);
        _questions.Verify(r => r.GetByCategoryIdAsync(categoryId), Times.Once);
    }
    [Fact]
    public async Task Handle_WhenCategoryExistsAndNoQuestions_ReturnsEmptyPage()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var emptyQuestions = new List<Question>();
        var mockQuestions = emptyQuestions.BuildMock();

        var emptyDtos = new List<QuestionSummaryDto>().BuildMock();

        _categories
            .Setup(r => r.ExistsAsync(categoryId))
            .ReturnsAsync(true);

        _questions
            .Setup(r => r.GetByCategoryIdAsync(categoryId))
            .ReturnsAsync(mockQuestions);

        _mapper
            .Setup(m => m.ProjectTo<QuestionSummaryDto>(
                It.IsAny<IQueryable<Question>>(),
                It.IsAny<object>()))
            .Returns(emptyDtos);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(BuildQuery(categoryId), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(SystemMessages.DATA_RETRIEVED, result.Message);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data.Items);
        Assert.Equal(0, result.Data.TotalCount);
    }

    [Fact]
    public async Task Handle_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        var questions = Enumerable.Range(1, 25)
            .Select(i => QuestionTestData.Question($"Q{i}", categoryId: categoryId))
            .ToList();

        var mockQuestions = questions.BuildMock();

        var mockDtos = questions
            .Select(q => new QuestionSummaryDto { Id = q.Id, Title = q.Title })
            .ToList()
            .BuildMock();

        _categories
            .Setup(r => r.ExistsAsync(categoryId))
            .ReturnsAsync(true);

        _questions
            .Setup(r => r.GetByCategoryIdAsync(categoryId))
            .ReturnsAsync(mockQuestions);

        _mapper
            .Setup(m => m.ProjectTo<QuestionSummaryDto>(
                It.IsAny<IQueryable<Question>>(),
                It.IsAny<object>()))
            .Returns(mockDtos);

        var handler = CreateHandler();

        // Act — page 2, size 10 → items 11..20
        var result = await handler.Handle(BuildQuery(categoryId, pageNumber: 2, pageSize: 10), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(SystemMessages.DATA_RETRIEVED, result.Message);
        Assert.NotNull(result.Data);
        Assert.Equal(25, result.Data.TotalCount);
        Assert.Equal(10, result.Data.Items.Count());
        Assert.Equal("Q11", result.Data.Items.First().Title);
        Assert.Equal("Q20", result.Data.Items.Last().Title);
    }

    [Fact]
    public async Task Handle_WhenQuestionRepositoryThrows_PropagatesException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

        _categories
            .Setup(r => r.ExistsAsync(categoryId))
            .ReturnsAsync(true);

        _questions
            .Setup(r => r.GetByCategoryIdAsync(categoryId))
            .ThrowsAsync(new InvalidOperationException("DB failure"));

        var handler = CreateHandler();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(BuildQuery(categoryId), CancellationToken.None));
    }


}