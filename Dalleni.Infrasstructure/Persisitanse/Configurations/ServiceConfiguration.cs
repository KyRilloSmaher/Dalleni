using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            builder.Property(x => x.RequiredDocuments).HasMaxLength(4000).IsRequired();
            builder.HasOne(x => x.OfficialEntity).WithMany(x => x.Services).HasForeignKey(x => x.OfficialEntityId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
