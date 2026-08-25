using Dalleni.Domin.Models;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.UnitTests.Modules.SavedQuestions.Repositories;

public class SavedQuestionRepositoryTests
{
   [Fact]
public async Task GetSavedQuestionsByUserIdAsync_ReturnsOnlyUserSavedQuestions()
{
    await using var context = DbContextFactory.CreateDbContext();
    var userId = Guid.NewGuid();
    var otherUserId = Guid.NewGuid();
    
    // Create users first
    var user1 = EndpointTestData.User();
    user1.Id = userId;
    var user2 = EndpointTestData.User(email: "other@example.com", userName: "other.user");
    user2.Id = otherUserId;
    
    await context.Users.AddRangeAsync(user1, user2);
    await context.SaveChangesAsync();
    
    // Create categories first
    var category1 =  Category.Create("Category 1" );
    var category2 = Category.Create("Category 2" );
    
    await context.Categories.AddRangeAsync(category1, category2);
    await context.SaveChangesAsync();
    
    // Create questions with proper category references
    var question1 = EndpointTestData.Question(userId: userId, categoryId: category1.Id);
    var question2 = EndpointTestData.Question(userId: otherUserId, categoryId: category2.Id);
    
    // Make sure the question has the Category navigation property set
    question1.Category = category1;
    question2.Category = category2;
    
    await context.Questions.AddRangeAsync(question1, question2);
    await context.SaveChangesAsync();
    
    // Now create saved questions referencing the existing questions
    var savedQuestion1 = SavedQuestion.Create(userId, question1.Id);
    var savedQuestion2 = SavedQuestion.Create(otherUserId, question2.Id);
    
    await context.SavedQuestions.AddRangeAsync(savedQuestion1, savedQuestion2);
    await context.SaveChangesAsync();
    
    var repository = new SavedQuestionRepository(context);

    var result = await repository.GetSavedQuestionsByUserIdAsync(userId);

    Assert.Single(result);
    var savedQuestion = result.Single();
    Assert.Equal(userId, savedQuestion.UserId);
    Assert.Equal(question1.Id, savedQuestion.QuestionId);
}
}