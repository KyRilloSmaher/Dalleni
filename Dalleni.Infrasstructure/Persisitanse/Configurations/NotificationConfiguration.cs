
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration: IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");

            // Primary Key
            builder.HasKey(notification => notification.Id);

            // Properties
            builder.Property(notification => notification.Id)
                .ValueGeneratedNever();

            builder.Property(notification => notification.RecipientId)
                .IsRequired();

            builder.Property(notification => notification.ActorId)
                .IsRequired(false);

            builder.Property(notification => notification.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(notification => notification.EntityType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(notification => notification.EntityId)
                .IsRequired(false);

            builder.Property(notification => notification.Channels)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(notification => notification.IsRead)
                .IsRequired();

            builder.Property(notification => notification.CreatedAt)
                .IsRequired();

            builder.Property(notification => notification.ReadAt)
                .IsRequired(false);

            // Recipient relationship
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(notification => notification.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Actor relationship
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(notification => notification.ActorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(notification => new
            {
                notification.RecipientId,
                notification.IsRead,
                notification.CreatedAt
            });

            builder.HasIndex(notification => new
            {
                notification.RecipientId,
                notification.CreatedAt
            });

            builder.HasIndex(notification => new
            {
                notification.EntityType,
                notification.EntityId
            });
        }
    }
}