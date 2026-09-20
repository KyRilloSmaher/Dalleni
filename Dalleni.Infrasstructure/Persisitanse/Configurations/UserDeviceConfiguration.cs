
using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persistence.Configurations
{
    public class UserDeviceConfiguration
        : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(
            EntityTypeBuilder<UserDevice> builder)
        {
            builder.ToTable("UserDevices");

            // Primary Key
            builder.HasKey(device => device.Id);

            builder.Property(device => device.Id)
                .ValueGeneratedNever();

            // Properties
            builder.Property(device => device.UserId)
                .IsRequired();

            builder.Property(device => device.DeviceToken)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(device => device.Platform)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(device => device.IsActive)
                .IsRequired();

            builder.Property(device => device.CreatedAt)
                .IsRequired();

            builder.Property(device => device.LastUsedAt)
                .IsRequired();

            // User relationship
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(device => device.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(device => device.UserId);

            builder.HasIndex(device => device.DeviceToken)
                .IsUnique();
        }
    }
}