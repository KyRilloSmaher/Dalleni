using Dalleni.Domin.Enums;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;

namespace Dalleni.UnitTests.Modules.Votes.Repositories;

public class VoteRepositoryTests
{
    [Fact]
    public async Task GetUserVoteForQuestionAsync_ReturnsMatchingVote()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var userId = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        await context.Votes.AddAsync(EndpointTestData.QuestionVote(userId, questionId));
        await context.SaveChangesAsync();
        var repository = new VoteRepository(context);

        var result = await repository.GetUserVoteForQuestionAsync(userId, questionId);

        Assert.NotNull(result);
        Assert.Equal(questionId, result!.QuestionId);
    }

    [Fact]
    public async Task CountAnswerVotesAsync_ReturnsCountForType()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var answerId = Guid.NewGuid();
        await context.Votes.AddRangeAsync(
            EndpointTestData.AnswerVote(answerId: answerId, type: VoteType.Upvote),
            EndpointTestData.AnswerVote(answerId: answerId, type: VoteType.Downvote),
            EndpointTestData.AnswerVote(answerId: answerId, type: VoteType.Upvote));
        await context.SaveChangesAsync();
        var repository = new VoteRepository(context);

        var count = await repository.CountAnswerVotesAsync(answerId, VoteType.Upvote);

        Assert.Equal(2, count);
    }
}

