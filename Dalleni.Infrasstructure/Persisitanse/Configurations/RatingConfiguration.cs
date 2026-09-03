using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
             builder.HasKey(e => e.Id);
            builder.Property(e => e.Value).IsRequired();
            builder.Property(e => e.Comment).HasMaxLength(500);
            builder.Property(e => e.UserName).HasMaxLength(100);
            builder.HasIndex(e => e.ServiceId);
            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.CreatedAt);
        }
    }
}
