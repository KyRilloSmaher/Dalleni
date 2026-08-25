using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Domin.Models;

namespace Dalleni.UnitTests.Shared.Builders;

public static class CategoryTestData
{
    public static Category Category(string name = "Programming")
    {
        return Dalleni.Domin.Models.Category.Create(name);
    }

    public static CategoryDto CategoryDto(Guid? id = null, string name = "Programming")
    {
        return new CategoryDto
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            QuestionCount = 0
        };
    }
}

