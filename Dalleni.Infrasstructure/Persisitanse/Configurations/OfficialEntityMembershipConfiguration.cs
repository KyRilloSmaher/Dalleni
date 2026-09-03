using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persistence.Configurations
{
    public class OfficialEntityMembershipConfiguration
        : IEntityTypeConfiguration<OfficialEntityMembership>
    {
        public void Configure(
            EntityTypeBuilder<OfficialEntityMembership> builder)
        {
            builder.ToTable("OfficialEntityMembership");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Role)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.OfficialEntityMemberships)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OfficialEntity)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.OfficialEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.OfficialEntityId
            })
            .IsUnique();
        }
    }
}