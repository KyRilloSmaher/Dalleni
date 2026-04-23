using Dalleni.Domin.Models;
using Dalleni.Domin.Models.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dalleni.Infrastructure.Persisitanse
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<QuestionTag> QuestionTags => Set<QuestionTag>();
        public DbSet<OfficialEntity> OfficialEntities => Set<OfficialEntity>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Vote> Votes => Set<Vote>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        public DbSet<ExternalLogin> ExternalLogins => Set<ExternalLogin>();
        public DbSet<SavedQuestion> SavedQuestions => Set<SavedQuestion>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            ApplySoftDeleteFilters(builder);
        }

        public override int SaveChanges()
        {
            ApplyAuditRules();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditRules()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletableEntity softDeletableEntity)
                {
                    entry.State = EntityState.Modified;
                    softDeletableEntity.Delete();
                }

                if (entry.Entity is DomainEntity)
                {
                    if (entry.State == EntityState.Added && entry.Property(nameof(DomainEntity.CreatedAt)).CurrentValue is null)
                    {
                        entry.Property(nameof(DomainEntity.CreatedAt)).CurrentValue = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(nameof(DomainEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;
                    }
                }
            }
        }

        private static void ApplySoftDeleteFilters(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    continue;
                }

                var parameter = Expression.Parameter(entityType.ClrType, "entity");
                var body = Expression.Equal(Expression.Property(parameter, nameof(ISoftDeletableEntity.IsDeleted)), Expression.Constant(false));
                entityType.SetQueryFilter(Expression.Lambda(body, parameter));
            }
        }
    }
}
