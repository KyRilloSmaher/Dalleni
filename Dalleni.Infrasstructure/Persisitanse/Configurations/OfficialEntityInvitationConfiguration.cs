using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persistence.Configurations
{
    public class OfficialEntityInvitationConfiguration
        : IEntityTypeConfiguration<OfficialEntityInvitation>
    {
        public void Configure(
            EntityTypeBuilder<OfficialEntityInvitation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(320);

            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.Role)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.IsAccepted)
                .IsRequired();

            builder.HasIndex(x => x.TokenHash)
                .IsUnique();

            builder.HasOne(x => x.OfficialEntity)
                .WithMany()
                .HasForeignKey(x => x.OfficialEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.InvitedByUser)
                .WithMany()
                .HasForeignKey(x => x.InvitedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}