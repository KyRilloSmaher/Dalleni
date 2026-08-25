using Dalleni.Domin.Models;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.UnitTests.Modules.Answers.Repositories;

public class AnswerRepositoryTests
{
    [Fact]
    public async Task GetByQuestionIdAsync_ReturnsOnlyAnswersForQuestion()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var userId = Guid.NewGuid();
        
        // Create a user first (if required by your domain)
        var user = EndpointTestData.User();
        user.Id = userId; 
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        // Create questions
        var question1 = EndpointTestData.Question(userId: userId, categoryId: Guid.NewGuid());
        var question2 = EndpointTestData.Question(userId: userId, categoryId: Guid.NewGuid());
        
        await context.Questions.AddRangeAsync(question1, question2);
        await context.SaveChangesAsync();
        
        // Create answers for question1
        var answer1 = EndpointTestData.Answer(questionId: question1.Id, userId: userId);
        var answer2 = EndpointTestData.Answer(questionId: question1.Id, userId: userId);
        var answer3 = EndpointTestData.Answer(questionId: question2.Id, userId: userId);
        
        await context.Answers.AddRangeAsync(answer1, answer2, answer3);
        await context.SaveChangesAsync();
        
        var repository = new AnswerRepository(context);

        var result = await repository.GetByQuestionIdAsync(question1.Id);

        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(question1.Id, a.QuestionId));
    }

    [Fact]
    public async Task GetAcceptedAnswerAsync_ReturnsAcceptedAnswerForQuestion()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var userId = Guid.NewGuid();
        
        // Create a user first
        var user = EndpointTestData.User();
        user.Id = userId;
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        // Create a question
        var question = EndpointTestData.Question(userId: userId, categoryId: Guid.NewGuid());
        await context.Questions.AddAsync(question);
        await context.SaveChangesAsync();
        
        // Create answers - one accepted, one not
        var acceptedAnswer = EndpointTestData.Answer(questionId: question.Id, userId: userId);
        acceptedAnswer.Accept();
        
        var notAcceptedAnswer = EndpointTestData.Answer(questionId: question.Id, userId: userId);
        notAcceptedAnswer.Unaccept();
        await context.Answers.AddRangeAsync(acceptedAnswer, notAcceptedAnswer);
        await context.SaveChangesAsync();
        
        var repository = new AnswerRepository(context);

        var result = await repository.GetAcceptedAnswerAsync(question.Id);

        Assert.NotNull(result);
        Assert.True(result.IsAccepted);
        Assert.Equal(question.Id, result.QuestionId);
    }
}