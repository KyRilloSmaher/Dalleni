using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class OfficialEntityConfiguration : IEntityTypeConfiguration<OfficialEntity>
    {
        public void Configure(EntityTypeBuilder<OfficialEntity> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            builder.HasOne(x => x.User).WithMany(x => x.OfficialEntities).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
