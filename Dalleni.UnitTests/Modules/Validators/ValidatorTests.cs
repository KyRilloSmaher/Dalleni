using Dalleni.Application.DTOs.Requests.Answers;
using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Application.DTOs.Requests.Questions;
using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.Validators.Answers;
using Dalleni.Application.Validators.Questions;
using Dalleni.Application.Validators.Ratings;
using Dalleni.Application.Validators.Users;
using FluentValidation.TestHelper;

namespace Dalleni.UnitTests.Modules.Validators;

public class ValidatorTests
{
    private readonly CreateAnswerRequestDtoValidator _answerValidator = new();
    private readonly CreateQuestionRequestDtoValidator _questionValidator = new();
    private readonly CreateRatingRequestDtoValidator _ratingValidator = new();
    private readonly SignUpRequestValidator _signUpValidator = new();

    [Fact]
    public void CreateAnswerRequestDtoValidator_Fails_WhenContentOrQuestionIdEmpty()
    {
        var dto = new CreateAnswerRequestDto { Content = "", QuestionId = Guid.Empty };
        var result = _answerValidator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Content);
        result.ShouldHaveValidationErrorFor(x => x.QuestionId);
    }

    [Fact]
    public void CreateAnswerRequestDtoValidator_Passes_WhenValid()
    {
        var dto = new CreateAnswerRequestDto { Content = "Valid answer text", QuestionId = Guid.NewGuid() };
        var result = _answerValidator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateQuestionRequestDtoValidator_Fails_WhenFieldsEmpty()
    {
        var dto = new CreateQuestionRequestDto { Title = "", Content = "", CategoryId = Guid.Empty };
        var result = _questionValidator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Content);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }

    [Fact]
    public void CreateQuestionRequestDtoValidator_Passes_WhenValid()
    {
        var dto = new CreateQuestionRequestDto
        {
            Title = "Valid Question Title",
            Content = "Valid Question Content",
            CategoryId = Guid.NewGuid(),
            Tags = new List<string> { "tag1", "tag2" }
        };
        var result = _questionValidator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateRatingRequestDtoValidator_Fails_WhenValueOutOfRange()
    {
        var dtoInvalidMin = new CreateRatingRequestDto { ServiceId = Guid.NewGuid(), Value = 0 };
        var dtoInvalidMax = new CreateRatingRequestDto { ServiceId = Guid.NewGuid(), Value = 6 };

        _ratingValidator.TestValidate(dtoInvalidMin).ShouldHaveValidationErrorFor(x => x.Value);
        _ratingValidator.TestValidate(dtoInvalidMax).ShouldHaveValidationErrorFor(x => x.Value);
    }

    [Fact]
    public void SignUpRequestValidator_Fails_WhenInvalidEmailOrPassword()
    {
        var dto = new SignUpRequest
        {
            FirstName = "",
            LastName = "",
            UserName = "",
            Email = "invalid-email",
            Password = "short"
        };
        var result = _signUpValidator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
