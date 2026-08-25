using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;

namespace Dalleni.UnitTests.Modules.Users.Repositories;

public class ApplicationUserRepositoryTests
{
    [Fact]
    public async Task GetByEmailAsync_ReturnsUserWithMatchingEmail()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var user = EndpointTestData.User(email: "user@example.com");
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        var repository = new ApplicationUserRepository(context);

        var result = await repository.GetByEmailAsync("user@example.com");

        Assert.NotNull(result);
        Assert.Equal(user.Id, result!.Id);
    }

    [Fact]
    public async Task SearchAsync_ReturnsUsersMatchingKeyword()
    {
        await using var context = DbContextFactory.CreateDbContext();
        await context.Users.AddRangeAsync(
            EndpointTestData.User(email: "ali@example.com", userName: "ali", fullName: "Ali Hassan"),
            EndpointTestData.User(email: "mona@example.com", userName: "mona", fullName: "Mona Farid"));
        await context.SaveChangesAsync();
        var repository = new ApplicationUserRepository(context);

        var result = await repository.SearchAsync("ali");

        Assert.Single(result);
        Assert.Equal("Ali Hassan", result.Single().FullName);
    }
}

