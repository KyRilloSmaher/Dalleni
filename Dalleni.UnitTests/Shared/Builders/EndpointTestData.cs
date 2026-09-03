using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;

namespace Dalleni.UnitTests.Shared.Builders;

public static class EndpointTestData
{
    public static ApplicationUser User(string email = "test@example.com", string userName = "test.user", string fullName = "Test User")
    {
        return ApplicationUser.Create(fullName, email, userName);
    }

    public static Question Question(Guid? userId = null, Guid? categoryId = null, string title = "Question title")
    {
        return Dalleni.Domin.Models.Question.Create(title, "Question content", userId ?? Guid.NewGuid(), categoryId ?? Guid.NewGuid());
    }

    public static Answer Answer(Guid? questionId = null, Guid? userId = null, string content = "Answer content")
    {
        return Dalleni.Domin.Models.Answer.CreateCommunityAnswer(content, questionId ?? Guid.NewGuid(), userId ?? Guid.NewGuid());
    }

    public static Tag Tag(string name = "dotnet")
    {
        return Dalleni.Domin.Models.Tag.Create(name);
    }

    public static Vote QuestionVote(Guid? userId = null, Guid? questionId = null, VoteType type = VoteType.Upvote)
    {
        return Vote.Create(userId ?? Guid.NewGuid(), type, questionId: questionId ?? Guid.NewGuid());
    }

    public static Vote AnswerVote(Guid? userId = null, Guid? answerId = null, VoteType type = VoteType.Upvote)
    {
        return Vote.Create(userId ?? Guid.NewGuid(), type, answerId: answerId ?? Guid.NewGuid());
    }

    public static AnswerDto AnswerDto(Guid? id = null, Guid? questionId = null)
    {
        return new AnswerDto
        {
            Id = id ?? Guid.NewGuid(),
            QuestionId = questionId ?? Guid.NewGuid(),
            Content = "Answer content",
            UserId = Guid.NewGuid(),
            AuthorName = "Test User"
        };
    }

    public static TokenReponseDto TokenResponse()
    {
        return new TokenReponseDto
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }

    public static QuestionDetailsResponseDto QuestionDetails(Guid? id = null)
    {
        return new QuestionDetailsResponseDto
        {
            Id = id ?? Guid.NewGuid(),
            Title = "Question title",
            Content = "Question content",
            CategoryId = Guid.NewGuid(),
            CategoryName = "Programming",
            UserId = Guid.NewGuid(),
            AuthorName = "Test User",
            CreatedAt = DateTime.UtcNow
        };
    }

    public static QuestionSummaryDto QuestionSummary(Guid? id = null)
    {
        return new QuestionSummaryDto
        {
            Id = id ?? Guid.NewGuid(),
            Title = "Question title",
            UserId = Guid.NewGuid(),
            AuthorName = "Test User",
            CreatedAt = DateTime.UtcNow
        };
    }

    public static PaginatedResult<QuestionDetailsResponseDto> PagedQuestions()
    {
        return PaginatedResult<QuestionDetailsResponseDto>.Success(new[] { QuestionDetails() }, 1, 1, 10);
    }

    public static PaginatedResult<TagDto> PagedTags()
    {
        return PaginatedResult<TagDto>.Success(new[] { TagDto() }, 1, 1, 10);
    }

    public static TagDto TagDto(Guid? id = null, string name = "dotnet")
    {
        return new TagDto
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Slug = name
        };
    }

    public static UserResponseDto UserResponse(Guid? id = null)
    {
        return new UserResponseDto
        {
            Id = id ?? Guid.NewGuid(),
            FullName = "Test User",
            UserName = "test.user",
            Email = "test@example.com"
        };
    }

    public static OfficialEntity OfficialEntity(string name = "Test Entity", string description = "Test Description")
    {
        return Dalleni.Domin.Models.OfficialEntity.Create(name, description);
    }

    public static OfficialEntityMembership Membership(Guid? entityId = null, Guid? userId = null, EntityRole role = EntityRole.Staff)
    {
        return OfficialEntityMembership.Create(entityId ?? Guid.NewGuid(), userId ?? Guid.NewGuid(), role);
    }

    public static OfficialEntityInvitation Invitation(Guid? entityId = null, Guid? invitedBy = null, string email = "test@example.com", EntityRole role = EntityRole.Staff)
    {
        return OfficialEntityInvitation.Create(entityId ?? Guid.NewGuid(), invitedBy ?? Guid.NewGuid(), email, role, "hashedtoken123", DateTime.UtcNow.AddDays(7));
    }

    public static Rating Rating(Guid? serviceId = null, Guid? userId = null, int value = 5, string? comment = "Great")
    {
        return Dalleni.Domin.Models.Rating.Create(serviceId ?? Guid.NewGuid(), userId ?? Guid.NewGuid(), value, comment);
    }

    public static SavedQuestion SavedQuestion(Guid? userId = null, Guid? questionId = null)
    {
        return Dalleni.Domin.Models.SavedQuestion.Create(userId ?? Guid.NewGuid(), questionId ?? Guid.NewGuid());
    }
}
