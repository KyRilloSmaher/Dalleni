using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;

namespace Dalleni.UnitTests.Modules.Categories.Repositories;

public class CategoryRepositoryTests
{
    [Fact]
    public async Task GetByNameAsync_WhenCategoryExists_ReturnsMatchingCategory()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var repository = new CategoryRepository(context);
        var category = CategoryTestData.Category("Programming");
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var result = await repository.GetByNameAsync("Program");

        Assert.NotNull(result);
        Assert.Equal("Programming", result!.Name);
    }

    [Fact]
    public async Task ExistsByNameAsync_WhenCategoryExists_ReturnsTrue()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var repository = new CategoryRepository(context);
        await context.Categories.AddAsync(CategoryTestData.Category("Programming"));
        await context.SaveChangesAsync();

        var exists = await repository.ExistsByNameAsync("Program");

        Assert.True(exists);
    }
}
