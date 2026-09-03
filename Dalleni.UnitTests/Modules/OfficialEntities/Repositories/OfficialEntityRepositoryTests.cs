using Dalleni.Domin.Enums;
using Dalleni.Domin.Models;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Dalleni.UnitTests.Shared.Builders;
using Dalleni.UnitTests.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.UnitTests.Modules.OfficialEntities.Repositories;

public class OfficialEntityRepositoryTests
{
    [Fact]
    public async Task GetVerifiedAsync_ReturnsOnlyVerifiedEntities()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        // Create users first
        var user1 = EndpointTestData.User();
        var user2 = EndpointTestData.User(email: "other@example.com", userName: "other.user");
        
        await context.Users.AddRangeAsync(user1, user2);
        await context.SaveChangesAsync();
        
        // Create verified and unverified entities
        var verifiedEntity = OfficialEntity.Create("Verified Entity", "Description");
        verifiedEntity.Verify();
        
        var unverifiedEntity = OfficialEntity.Create("Unverified Entity", "Description");
        
        await context.OfficialEntities.AddRangeAsync(verifiedEntity, unverifiedEntity);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityRepository(context);

        var result = await repository.GetVerifiedAsync();

        var entities = await result.ToListAsync();
        Assert.Single(entities);
        var entity = entities.Single();
        Assert.Equal(verifiedEntity.Id, entity.Id);
        Assert.True(entity.IsVerified);
    }

    [Fact]
    public async Task SearchAsync_ReturnsEntitiesMatchingKeyword()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        var user1 = EndpointTestData.User();
        var user2 = EndpointTestData.User(email: "other@example.com", userName: "other.user");
        
        await context.Users.AddRangeAsync(user1, user2);
        await context.SaveChangesAsync();
        
        var entity1 = OfficialEntity.Create("Tech Solutions Inc", "Technology consulting services");
        entity1.Verify();
        
        var entity2 = OfficialEntity.Create("Healthcare Partners", "Medical services provider");
        entity2.Verify();
        
        await context.OfficialEntities.AddRangeAsync(entity1, entity2);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityRepository(context);

        var result = await repository.SearchAsync("Tech");

        Assert.Single(result);
        var entity = result.Single();
        Assert.Equal(entity1.Id, entity.Id);
        Assert.Contains("Tech", entity.Name);
    }

    [Fact]
    public async Task ExistsByNameAsync_ReturnsTrueWhenNameExists()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        var user = EndpointTestData.User();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var entity = OfficialEntity.Create("Unique Entity Name", "Description");
        
        await context.OfficialEntities.AddAsync(entity);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityRepository(context);

        var exists = await repository.ExistsByNameAsync("Unique Entity Name");

        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsByNameAsync_ReturnsFalseWhenNameDoesNotExist()
    {
        await using var context = DbContextFactory.CreateDbContext();
        var repository = new OfficialEntityRepository(context);

        var exists = await repository.ExistsByNameAsync("NonExistentName");

        Assert.False(exists);
    }
}

public class OfficialEntityMembershipRepositoryTests
{
    [Fact]
    public async Task GetByUserAndEntityAsync_ReturnsMembershipWhenExists()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        var user = EndpointTestData.User();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var entity = OfficialEntity.Create("Test Entity", "Description");
        await context.OfficialEntities.AddAsync(entity);
        await context.SaveChangesAsync();
        
        var membership = OfficialEntityMembership.Create(entity.Id, user.Id, EntityRole.Staff);
        await context.OfficialEntityMemberships.AddAsync(membership);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityMembershipRepository(context);

        var result = await repository.GetByUserAndEntityAsync(user.Id, entity.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.UserId);
        Assert.Equal(entity.Id, result.OfficialEntityId);
    }

    [Fact]
    public async Task GetEntitiesForUserAsync_ReturnsOnlyActiveEntities()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        var user = EndpointTestData.User();
        var otherUser = EndpointTestData.User(email: "other@example.com", userName: "other.user");
        await context.Users.AddRangeAsync(user, otherUser);
        await context.SaveChangesAsync();
        
        var entity1 = OfficialEntity.Create("Active Entity", "Description");
        var entity2 = OfficialEntity.Create("Inactive Entity", "Description");
        var entity3 = OfficialEntity.Create("Deleted Entity", "Description");
        entity3.Delete();
        
        await context.OfficialEntities.AddRangeAsync(entity1, entity2, entity3);
        await context.SaveChangesAsync();
        
        var membership1 = OfficialEntityMembership.Create(entity1.Id, user.Id, EntityRole.Staff);
        
        var membership2 = OfficialEntityMembership.Create(entity2.Id, user.Id, EntityRole.Staff);
        membership2.Deactivate();
        
        var membership3 = OfficialEntityMembership.Create(entity3.Id, user.Id, EntityRole.Staff);
        
        await context.OfficialEntityMemberships.AddRangeAsync(membership1, membership2, membership3);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityMembershipRepository(context);

        var result = await repository.GetEntitiesForUserAsync(user.Id);

        Assert.Single(result);
        var entity = result.Single();
        Assert.Equal(entity1.Id, entity.Id);
        Assert.False(entity.IsDeleted);
    }
}

public class OfficialEntityInvitationRepositoryTests
{
    [Fact]
    public async Task GetByTokenHashAsync_ReturnsInvitationWhenExists()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        var user = EndpointTestData.User();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var entity = OfficialEntity.Create("Test Entity", "Description");
        await context.OfficialEntities.AddAsync(entity);
        await context.SaveChangesAsync();
        
        var tokenHash = "hashed_token_123";
        var invitation = OfficialEntityInvitation.Create(entity.Id, user.Id, "test@test.com", EntityRole.Staff, tokenHash, DateTime.UtcNow.AddDays(7));
        await context.OfficialEntityInvitations.AddAsync(invitation);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityInvitationRepository(context);

        var result = await repository.GetByTokenHashAsync(tokenHash);

        Assert.NotNull(result);
        Assert.Equal(tokenHash, result.TokenHash);
        Assert.Equal(entity.Id, result.OfficialEntityId);
    }

    [Fact]
    public async Task GetPendingInvitationAsync_ReturnsPendingInvitation()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        var user = EndpointTestData.User();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var entity = OfficialEntity.Create("Test Entity", "Description");
        await context.OfficialEntities.AddAsync(entity);
        await context.SaveChangesAsync();
        
        var email = "test@test.com";
        
        var pendingInvitation = OfficialEntityInvitation.Create(entity.Id, user.Id, email, EntityRole.Staff, "hashed_token_1", DateTime.UtcNow.AddDays(7));
        
        var acceptedInvitation = OfficialEntityInvitation.Create(entity.Id, user.Id, email, EntityRole.Staff, "hashed_token_2", DateTime.UtcNow.AddDays(7));
        acceptedInvitation.Accept();
        
        await context.OfficialEntityInvitations.AddRangeAsync(pendingInvitation, acceptedInvitation);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityInvitationRepository(context);

        var result = await repository.GetPendingInvitationAsync(entity.Id, email);

        Assert.NotNull(result);
        Assert.Equal(pendingInvitation.Id, result.Id);
        Assert.False(result.IsAccepted);
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task GetPendingInvitationAsync_ReturnsNullWhenNoPendingInvitation()
    {
        await using var context = DbContextFactory.CreateDbContext();
        
        var user = EndpointTestData.User();
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        var entity = OfficialEntity.Create("Test Entity", "Description");
        await context.OfficialEntities.AddAsync(entity);
        await context.SaveChangesAsync();
        
        var repository = new OfficialEntityInvitationRepository(context);

        var result = await repository.GetPendingInvitationAsync(entity.Id, "nonexistent@test.com");

        Assert.Null(result);
    }
}
