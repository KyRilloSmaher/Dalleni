using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;

namespace Dalleni.UnitTests.Modules.Tags.Repositories;

public class TagRepositoryTests
{
    [Fact]
    public async Task GetByNormalizedNameAsync_NormalizesInputBeforeLookup()
    {
        await using var context = DbContextFactory.CreateDbContext();
        await context.Tags.AddAsync(EndpointTestData.Tag("Dot Net"));
        await context.SaveChangesAsync();
        var repository = new TagRepository(context);

        var result = await repository.GetByNormalizedNameAsync("  dot net  ");

        Assert.NotNull(result);
        Assert.Equal("Dot Net", result!.Name);
    }

    [Fact]
    public async Task ExistsByNormalizedNameAsync_ReturnsTrueWhenTagExists()
    {
        await using var context = DbContextFactory.CreateDbContext();
        await context.Tags.AddAsync(EndpointTestData.Tag("Dot Net"));
        await context.SaveChangesAsync();
        var repository = new TagRepository(context);

        var exists = await repository.ExistsByNormalizedNameAsync("DOT NET");

        Assert.True(exists);
    }
}

