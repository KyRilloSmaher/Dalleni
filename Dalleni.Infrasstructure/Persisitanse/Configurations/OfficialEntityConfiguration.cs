using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class OfficialEntityConfiguration : IEntityTypeConfiguration<OfficialEntity>
    {
        public void Configure(EntityTypeBuilder<OfficialEntity> builder)
        {
           builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(x => x.LogoUrl)
                .HasMaxLength(1000);

            builder.Property(x => x.WebsiteUrl)
                .HasMaxLength(1000);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasMany(x => x.Members)
                .WithOne(x => x.OfficialEntity)
                .HasForeignKey(x => x.OfficialEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Services)
                .WithOne(x => x.OfficialEntity)
                .HasForeignKey(x => x.OfficialEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
