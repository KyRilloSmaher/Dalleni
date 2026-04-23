using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");
            builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ProfileImageUrl).HasMaxLength(1000);
            builder.Property(x => x.RefreshToken).HasMaxLength(2000);
            builder.Property(x => x.OTP).HasMaxLength(500);
        }
    }
}
