using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class ExternalLoginConfiguration : IEntityTypeConfiguration<ExternalLogin>
    {
        public void Configure(EntityTypeBuilder<ExternalLogin> builder)
        {
            builder.Property(x => x.Provider).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ProviderKey).HasMaxLength(200).IsRequired();
            builder.HasIndex(x => new { x.Provider, x.ProviderKey }).IsUnique();
            builder.HasOne(x => x.User).WithMany(x => x.ExternalLogins).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
