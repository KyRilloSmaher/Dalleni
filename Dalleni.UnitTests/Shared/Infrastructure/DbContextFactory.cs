using Dalleni.Infrastructure.Persisitanse;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.UnitTests.Shared.Infrastructure;

public static class DbContextFactory
{
    public static ApplicationDbContext CreateDbContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}

