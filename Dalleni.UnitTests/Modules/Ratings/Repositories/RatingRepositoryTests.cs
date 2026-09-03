using Dalleni.Domin.Models;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;


namespace Dalleni.UnitTests.Modules.Ratings.Repositories;


public static class RatingTestData
{
    public static Rating Rating(
        Guid serviceId,
        Guid userId,
        int value,
        string comment,
        string userName = "TestUser")
    {
        var rating = Dalleni.Domin.Models.Rating.Create(serviceId, userId, value, comment, userName);
        
        // Ensure CreatedAt is set for testing
        if (rating.CreatedAt == default)
        {
            // Use reflection or add a method to set CreatedAt
            SetCreatedAt(rating, DateTime.UtcNow);
        }
        
        return rating;
    }

    public static void SetCreatedAt(this Rating rating, DateTime createdAt)
    {
        var propertyInfo = typeof(Rating).GetProperty("CreatedAt");
        if (propertyInfo != null && propertyInfo.CanWrite)
        {
            propertyInfo.SetValue(rating, createdAt);
        }
    }

    public static void SetUpdatedAt(this Rating rating, DateTime updatedAt)
    {
        var propertyInfo = typeof(Rating).GetProperty("UpdatedAt");
        if (propertyInfo != null && propertyInfo.CanWrite)
        {
            propertyInfo.SetValue(rating, updatedAt);
        }
    }

    public static List<Rating> CreateRatingsForService(
        Guid serviceId, 
        int count = 5)
    {
        var ratings = new List<Rating>();
        var random = new Random();
        
        for (int i = 0; i < count; i++)
        {
            var rating = Rating(
                serviceId,
                Guid.NewGuid(),
                random.Next(1, 6),
                $"Test comment {i + 1}",
                $"User{i + 1}"
            );
            ratings.Add(rating);
        }
        
        return ratings;
    }

    public static List<Rating> CreateRatingsForUser(
        Guid userId, 
        int count = 3)
    {
        var ratings = new List<Rating>();
        var random = new Random();
        
        for (int i = 0; i < count; i++)
        {
            var rating = Rating(
                Guid.NewGuid(),
                userId,
                random.Next(1, 6),
                $"User rating {i + 1}",
                $"TestUser"
            );
            ratings.Add(rating);
        }
        
        return ratings;
    }
}

public class RatingRepositoryTests
{
    [Fact]
    public async Task GetAverageRatingForServiceAsync_ExcludesDeletedRatings()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        // Create ratings
        var rating1 = RatingTestData.Rating(serviceId, userId, 5, "Excellent!");
        var rating2 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 4, "Good!");
        var rating3 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 3, "Average!");
        var rating4 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 2, "Poor!");
        rating4.Delete(); // Soft delete this rating
        
        await context.Ratings.AddRangeAsync(rating1, rating2, rating3, rating4);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetAverageRatingForServiceAsync(serviceId);

        // Assert
        // Average of 5, 4, 3 = 4.0 (deleted rating 2 is excluded)
        Assert.Equal(4.0, result);
    }

    [Fact]
    public async Task GetAverageRatingForServiceAsync_WhenNoRatings_ReturnsZero()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetAverageRatingForServiceAsync(serviceId);

        // Assert
        Assert.Equal(0.0, result);
    }

    [Fact]
    public async Task GetRatingsForServiceAsync_ExcludesDeletedRatings()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        // Create ratings
        var rating1 = RatingTestData.Rating(serviceId, userId, 5, "Excellent!");
        var rating2 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 4, "Good!");
        var rating3 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 3, "Average!");
        var rating4 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 2, "Poor!");
        rating4.Delete(); // Soft delete this rating
        
        await context.Ratings.AddRangeAsync(rating1, rating2, rating3, rating4);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetRatingsForServiceAsync(serviceId);

        // Assert
        Assert.Equal(3, result.Count());
        Assert.DoesNotContain(result, r => r.IsDeleted);
        Assert.Contains(result, r => r.Id == rating1.Id);
        Assert.Contains(result, r => r.Id == rating2.Id);
        Assert.Contains(result, r => r.Id == rating3.Id);
        Assert.DoesNotContain(result, r => r.Id == rating4.Id);
    }

    [Fact]
    public async Task GetRatingsForServiceAsync_OrdersByCreatedAtDescending()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        // Create ratings with different creation times
        var rating1 = RatingTestData.Rating(serviceId, userId, 5, "First");
        rating1.SetCreatedAt(DateTime.UtcNow.AddDays(-3));
        
        var rating2 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 4, "Second");
        rating2.SetCreatedAt(DateTime.UtcNow.AddDays(-2));
        
        var rating3 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 3, "Third");
        rating3.SetCreatedAt(DateTime.UtcNow.AddDays(-1));
        
        await context.Ratings.AddRangeAsync(rating1, rating2, rating3);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetRatingsForServiceAsync(serviceId);
        var resultList = result.ToList();

        // Assert
        Assert.Equal(3, resultList.Count);
        Assert.Equal(rating3.Id, resultList[0].Id); // Most recent first
        Assert.Equal(rating2.Id, resultList[1].Id);
        Assert.Equal(rating1.Id, resultList[2].Id);
    }

    [Fact]
    public async Task GetRatingsForServiceAsync_WhenNoRatings_ReturnsEmptyList()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetRatingsForServiceAsync(serviceId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserRatingForServiceAsync_ReturnsUserRating()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        var rating = RatingTestData.Rating(serviceId, userId, 5, "Excellent!");
        var otherRating = RatingTestData.Rating(serviceId, Guid.NewGuid(), 4, "Other user");
        
        await context.Ratings.AddRangeAsync(rating, otherRating);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetUserRatingForServiceAsync(serviceId, userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(rating.Id, result.Id);
        Assert.Equal(userId, result.UserId);
        Assert.Equal(5, result.Value);
        Assert.Equal("Excellent!", result.Comment);
    }

    [Fact]
    public async Task GetUserRatingForServiceAsync_ExcludesDeletedRatings()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        var rating = RatingTestData.Rating(serviceId, userId, 5, "Excellent!");
        rating.Delete(); // Soft delete
        
        await context.Ratings.AddAsync(rating);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetUserRatingForServiceAsync(serviceId, userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserRatingForServiceAsync_WhenNoRating_ReturnsNull()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetUserRatingForServiceAsync(serviceId, userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserRatings_ReturnsAllRatingsForUser()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var userId = Guid.NewGuid();
        
        var rating1 = RatingTestData.Rating(Guid.NewGuid(), userId, 5, "Rating 1");
        var rating2 = RatingTestData.Rating(Guid.NewGuid(), userId, 4, "Rating 2");
        var rating3 = RatingTestData.Rating(Guid.NewGuid(), userId, 3, "Rating 3");
        var otherRating = RatingTestData.Rating(Guid.NewGuid(), Guid.NewGuid(), 5, "Other user");
        
        await context.Ratings.AddRangeAsync(rating1, rating2, rating3, otherRating);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetUserRatings(userId);
        var resultList = result.ToList();

        // Assert
        Assert.Equal(3, resultList.Count);
        Assert.All(resultList, r => Assert.Equal(userId, r.UserId));
        Assert.Contains(resultList, r => r.Id == rating1.Id);
        Assert.Contains(resultList, r => r.Id == rating2.Id);
        Assert.Contains(resultList, r => r.Id == rating3.Id);
        Assert.DoesNotContain(resultList, r => r.Id == otherRating.Id);
    }

    [Fact]
    public async Task GetUserRatings_ExcludesDeletedRatings()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var userId = Guid.NewGuid();
        
        var rating1 = RatingTestData.Rating(Guid.NewGuid(), userId, 5, "Active rating");
        var rating2 = RatingTestData.Rating(Guid.NewGuid(), userId, 4, "Deleted rating");
        rating2.Delete(); // Soft delete
        
        await context.Ratings.AddRangeAsync(rating1, rating2);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetUserRatings(userId);

        // Assert
        Assert.Single(result);
        Assert.Equal(rating1.Id, result.Single().Id);
        Assert.DoesNotContain(result, r => r.Id == rating2.Id);
    }

    [Fact]
    public async Task GetUserRatings_WhenNoRatings_ReturnsEmptyList()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var userId = Guid.NewGuid();
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetUserRatings(userId);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddRating_PersistsToDatabase()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var rating = RatingTestData.Rating(serviceId, userId, 5, "New rating");
        
        var repository = new RatingRepository(context);

        // Act
        await repository.AddAsync(rating);
        await context.SaveChangesAsync();

        // Assert
        var savedRating = await context.Ratings.FindAsync(rating.Id);
        Assert.NotNull(savedRating);
        Assert.Equal(rating.Id, savedRating.Id);
        Assert.Equal(5, savedRating.Value);
        Assert.Equal("New rating", savedRating.Comment);
        Assert.False(savedRating.IsDeleted);
    }

    [Fact]
    public async Task UpdateRating_UpdatesInDatabase()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var rating = RatingTestData.Rating(serviceId, userId, 3, "Original comment");
        
        await context.Ratings.AddAsync(rating);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        rating.Update(5, "Updated comment");;
        await context.SaveChangesAsync();

        // Assert
        var updatedRating = await context.Ratings.FindAsync(rating.Id);
        Assert.NotNull(updatedRating);
        Assert.Equal(5, updatedRating.Value);
        Assert.Equal("Updated comment", updatedRating.Comment);
        Assert.NotNull(updatedRating.UpdatedAt);
    }

    [Fact]
    public async Task DeleteRating_SoftDeletesInDatabase()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var rating = RatingTestData.Rating(serviceId, userId, 4, "To be deleted");
        
        await context.Ratings.AddAsync(rating);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        rating.Delete();

        await context.SaveChangesAsync();

        // Assert
        var deletedRating = await context.Ratings.FindAsync(rating.Id);
        Assert.NotNull(deletedRating);
        Assert.True(deletedRating.IsDeleted);
        Assert.NotNull(deletedRating.UpdatedAt);
        
        // Verify it's not returned in queries
        var ratings = await repository.GetRatingsForServiceAsync(serviceId);
        Assert.DoesNotContain(ratings, r => r.Id == rating.Id);
    }

    [Fact]
    public async Task RestoreRating_RestoresDeletedRating()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var rating = RatingTestData.Rating(serviceId, userId, 4, "To be restored");
        rating.Delete(); // Initially delete
        
        await context.Ratings.AddAsync(rating);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        rating.Restore();
        await context.SaveChangesAsync();

        // Assert
        var restoredRating = await context.Ratings.FindAsync(rating.Id);
        Assert.NotNull(restoredRating);
        Assert.False(restoredRating.IsDeleted);
        
        // Verify it's returned in queries
        var ratings = await repository.GetRatingsForServiceAsync(serviceId);
        Assert.Contains(ratings, r => r.Id == rating.Id);
    }

    [Fact]
    public async Task GetAverageRatingForServiceAsync_UpdatesAfterNewRating()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        
        // Initial ratings
        var rating1 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 4, "Rating 1");
        var rating2 = RatingTestData.Rating(serviceId, Guid.NewGuid(), 5, "Rating 2");
        
        await context.Ratings.AddRangeAsync(rating1, rating2);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);
        
        // Initial average
        var initialAverage = await repository.GetAverageRatingForServiceAsync(serviceId);
        Assert.Equal(4.5, initialAverage);

        // Act - Add new rating
        var newRating = RatingTestData.Rating(serviceId, Guid.NewGuid(), 3, "New rating");
        await repository.AddAsync(newRating);
        await context.SaveChangesAsync();

        // Assert
        var newAverage = await repository.GetAverageRatingForServiceAsync(serviceId);
        // New average: (4 + 5 + 3) / 3 = 4.0
        Assert.Equal(4.0, newAverage);
    }

    [Fact]
    public async Task GetRatingsForServiceAsync_IncludesAllProperties()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        
        var rating = RatingTestData.Rating(serviceId, userId, 5, "Test comment", "TestUser");
        
        await context.Ratings.AddAsync(rating);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetRatingsForServiceAsync(serviceId);
        var savedRating = result.First();

        // Assert
        Assert.NotEqual(Guid.Empty, savedRating.Id);
        Assert.Equal(serviceId, savedRating.ServiceId);
        Assert.Equal(userId, savedRating.UserId);
        Assert.Equal(5, savedRating.Value);
        Assert.Equal("Test comment", savedRating.Comment);
        Assert.Equal("TestUser", savedRating.UserName);
        Assert.False(savedRating.IsDeleted);
        Assert.NotNull(savedRating.CreatedAt);
        Assert.Null(savedRating.UpdatedAt);
    }

    [Fact]
    public async Task MultipleRatingsForSameService_ReturnsAll()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var serviceId = Guid.NewGuid();
        var count = 5;
        
        var ratings = new List<Rating>();
        for (int i = 0; i < count; i++)
        {
            var rating = RatingTestData.Rating(
                serviceId, 
                Guid.NewGuid(), 
                i + 1, 
                $"Rating {i + 1}"
            );
            ratings.Add(rating);
        }
        
        await context.Ratings.AddRangeAsync(ratings);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetRatingsForServiceAsync(serviceId);

        // Assert
        Assert.Equal(count, result.Count());
        foreach (var rating in ratings)
        {
            Assert.Contains(result, r => r.Id == rating.Id);
        }
    }

    [Fact]
    public async Task GetUserRatings_OrdersByCreatedAtDescending()
    {
        // Arrange
        await using var context = DbContextFactory.CreateDbContext();
        var userId = Guid.NewGuid();
        
        var rating1 = RatingTestData.Rating(Guid.NewGuid(), userId, 5, "Oldest");
        rating1.SetCreatedAt(DateTime.UtcNow.AddDays(-3));
        
        var rating2 = RatingTestData.Rating(Guid.NewGuid(), userId, 4, "Middle");
        rating2.SetCreatedAt(DateTime.UtcNow.AddDays(-2));
        
        var rating3 = RatingTestData.Rating(Guid.NewGuid(), userId, 3, "Newest");
        rating3.SetCreatedAt(DateTime.UtcNow.AddDays(-1));
        
        await context.Ratings.AddRangeAsync(rating1, rating2, rating3);
        await context.SaveChangesAsync();
        
        var repository = new RatingRepository(context);

        // Act
        var result = await repository.GetUserRatings(userId);
        var resultList = result.ToList();

        // Assert
        Assert.Equal(3, resultList.Count);
        Assert.Equal(rating3.Id, resultList[0].Id); // Newest first
        Assert.Equal(rating2.Id, resultList[1].Id);
        Assert.Equal(rating1.Id, resultList[2].Id);
    }
}