using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;

public static class QuestionTestData
{
    public static Question Question(string title = "Sample question", Guid? categoryId = null, Guid? userId = null) =>
       EndpointTestData.Question(title: title, categoryId: categoryId ?? Guid.NewGuid(), userId: userId ?? Guid.NewGuid());
}

public static class PagedRequestTestData
{
    public static PagedRequest Valid(int pageNumber = 1, int pageSize = 10) =>
        new() { PageNumber = pageNumber, PageSize = pageSize };
}