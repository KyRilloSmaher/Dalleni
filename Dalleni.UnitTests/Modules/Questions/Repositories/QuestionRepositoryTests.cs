using Dalleni.Domin.Models;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;

namespace Dalleni.UnitTests.Modules.Questions.Repositories;

public class QuestionRepositoryTests
{
    [Fact]
    public async Task GetOpenQuestionsAsync_ExcludesClosedQuestions()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var user = EndpointTestData.User();
        var category = CategoryTestData.Category("Programming");
        await context.Users.AddAsync(user);
        await context.Categories.AddAsync(category);
        var openQuestion = EndpointTestData.Question(user.Id, category.Id, "Open question");
        var closedQuestion = EndpointTestData.Question(user.Id, category.Id, "Closed question");
        closedQuestion.Close();
        await context.Questions.AddRangeAsync(openQuestion, closedQuestion);
        await context.SaveChangesAsync();
        var repository = new QuestionRepository(context);

        var result = await repository.GetOpenQuestionsAsync();

        Assert.Single(result);
        Assert.False(result.Single().IsClosed);
    }

    [Fact]
public async Task GetByTagIdAsync_ReturnsQuestionsWithMatchingTag()
{
    await using var context = DbContextFactory.CreateDbContext();
    

    var user = EndpointTestData.User();
    var category = CategoryTestData.Category("Programming");
    await context.Users.AddAsync(user);
    await context.Categories.AddAsync(category);
    

    var tag = EndpointTestData.Tag();
    
    var question = EndpointTestData.Question(user.Id, category.Id, "Test question");
    
    await context.Tags.AddAsync(tag);
    await context.Questions.AddAsync(question);
    await context.QuestionTags.AddAsync(QuestionTag.Create(question.Id, tag.Id));
    await context.SaveChangesAsync();
    
    var repository = new QuestionRepository(context);

    var query = await repository.GetByTagIdAsync(tag.Id);

    Assert.Single(query);
    Assert.Equal(question.Id, query.Single().Id);
}
}
