using Dalleni.Domin.Enums;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models;
using Dalleni.UnitTests.Shared.Builders;

namespace Dalleni.UnitTests.Modules.Domain;

public class DomainEntityTests
{
    [Fact]
    public void DomainGuard_AgainstNullOrWhiteSpace_ThrowsBadRequestException_WhenNullOrEmpty()
    {
        Assert.Throws<BadRequestException>(() => DomainGuard.AgainstNullOrWhiteSpace(null!, "param"));
        Assert.Throws<BadRequestException>(() => DomainGuard.AgainstNullOrWhiteSpace("", "param"));
        Assert.Throws<BadRequestException>(() => DomainGuard.AgainstNullOrWhiteSpace("   ", "param"));
    }

    [Fact]
    public void DomainGuard_AgainstNullOrWhiteSpace_ReturnsValue_WhenValid()
    {
        var result = DomainGuard.AgainstNullOrWhiteSpace("Valid text", "param");
        Assert.Equal("Valid text", result);
    }

    [Fact]
    public void DomainGuard_AgainstEmpty_ThrowsBadRequestException_WhenGuidIsEmpty()
    {
        Assert.Throws<BadRequestException>(() => DomainGuard.AgainstEmpty(Guid.Empty, "param"));
    }

    [Fact]
    public void DomainGuard_AgainstNegative_ThrowsBadRequestException_WhenNegative()
    {
        Assert.Throws<BadRequestException>(() => DomainGuard.AgainstNegative(-1, "param"));
    }

    [Fact]
    public void DomainGuard_AgainstPast_ThrowsBadRequestException_WhenInPast()
    {
        Assert.Throws<BadRequestException>(() => DomainGuard.AgainstPast(DateTime.UtcNow.AddMinutes(-10), "param"));
    }

    [Fact]
    public void Question_Create_InitializesPropertiesCorrectly()
    {
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var question = Question.Create("Title", "Content", userId, categoryId);

        Assert.NotEqual(Guid.Empty, question.Id);
        Assert.Equal("Title", question.Title);
        Assert.Equal("Content", question.Content);
        Assert.Equal(userId, question.UserId);
        Assert.Equal(categoryId, question.CategoryId);
        Assert.False(question.IsClosed);
        Assert.Equal(0, question.Views);
        Assert.Equal(0, question.UpVotes);
        Assert.Equal(0, question.DownVotes);
    }

    [Fact]
    public void Question_AddTag_ThrowsDomainException_WhenExceedingMaxTags()
    {
        var question = EndpointTestData.Question();
        for (int i = 1; i <= 5; i++)
        {
            question.AddTag(Tag.Create($"tag{i}"));
        }

        var extraTag = Tag.Create("tag6");
        var ex = Assert.Throws<DomainException>(() => question.AddTag(extraTag));
        Assert.Contains("Maximum 5 tags allowed", ex.Message);
    }

    [Fact]
    public void Question_ApplyVote_AdjustsScoreAndCounts()
    {
        var question = EndpointTestData.Question();

        question.ApplyVote(VoteType.Upvote);
        Assert.Equal(1, question.UpVotes);
        Assert.Equal(1.5, question.Score);

        question.ApplyVote(VoteType.Downvote);
        Assert.Equal(1, question.DownVotes);
        Assert.Equal(0.5, question.Score);
    }

    [Fact]
    public void Question_CloseAndReopen_UpdatesState()
    {
        var question = EndpointTestData.Question();

        question.Close();
        Assert.True(question.IsClosed);

        question.Reopen();
        Assert.False(question.IsClosed);
    }

    [Fact]
    public void Answer_CreateCommunityAnswer_InitializesCorrectly()
    {
        var questionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var answer = Answer.CreateCommunityAnswer("Test Answer", questionId, userId);

        Assert.Equal("Test Answer", answer.Content);
        Assert.Equal(questionId, answer.QuestionId);
        Assert.Equal(userId, answer.UserId);
        Assert.Equal(AnswerType.Community, answer.Type);
        Assert.False(answer.IsAccepted);
    }

    [Fact]
    public void Answer_AcceptAndUnaccept_UpdatesAcceptedState()
    {
        var answer = EndpointTestData.Answer();

        answer.Accept();
        Assert.True(answer.IsAccepted);

        answer.Unaccept();
        Assert.False(answer.IsAccepted);
    }

    [Fact]
    public void Vote_Create_ThrowsDomainException_WhenBothOrNeitherTargetsSet()
    {
        var userId = Guid.NewGuid();
        var qId = Guid.NewGuid();
        var aId = Guid.NewGuid();

        // Neither
        Assert.Throws<DomainException>(() => Vote.Create(userId, VoteType.Upvote));
        // Both
        Assert.Throws<DomainException>(() => Vote.Create(userId, VoteType.Upvote, questionId: qId, answerId: aId));
    }

    [Fact]
    public void Rating_Create_ThrowsException_WhenValueOutOfRange()
    {
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        Assert.Throws<BadRequestException>(() => Rating.Create(serviceId, userId, -1));
        Assert.Throws<BadRequestException>(() => Rating.Create(serviceId, userId, 6));

        var validRating = Rating.Create(serviceId, userId, 5, "Excellent");
        Assert.Equal(5, validRating.Value);
        Assert.Equal("Excellent", validRating.Comment);
    }

    [Fact]
    public void OfficialEntityMembership_Permissions_CheckRoleMatrix()
    {
        var entityId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var owner = OfficialEntityMembership.Create(entityId, userId, EntityRole.Owner);
        var member = OfficialEntityMembership.Create(entityId, userId, EntityRole.Staff);

        Assert.True(owner.CanPublishPosts());
        Assert.True(owner.CanManageServices());
        Assert.True(owner.CanAnswerOfficially());

        Assert.False(member.CanManageMembers());
        Assert.False(member.CanPublishPosts());
        Assert.False(member.CanManageServices());
    }

    [Fact]
    public void OfficialEntityInvitation_Accept_ThrowsDomainException_WhenAlreadyAcceptedOrExpired()
    {
        var entityId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var validInvitation = OfficialEntityInvitation.Create(entityId, userId, "user@test.com", EntityRole.Staff, "hash", DateTime.UtcNow.AddMinutes(30));
        validInvitation.Accept();
        Assert.True(validInvitation.IsAccepted);

        // Cannot accept twice
        Assert.Throws<DomainException>(() => validInvitation.Accept());

        // Expired invitation creation throws
        Assert.Throws<DomainException>(() => OfficialEntityInvitation.Create(entityId, userId, "user@test.com", EntityRole.Staff, "hash", DateTime.UtcNow.AddMinutes(-5)));
    }
}
