using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.HasOne(x => x.User).WithMany(x => x.Votes).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Question).WithMany().HasForeignKey(x => x.QuestionId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Answer).WithMany(x => x.Votes).HasForeignKey(x => x.AnswerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new { x.UserId, x.QuestionId }).IsUnique().HasFilter("[QuestionId] IS NOT NULL");
            builder.HasIndex(x => new { x.UserId, x.AnswerId }).IsUnique().HasFilter("[AnswerId] IS NOT NULL");
        }
    }
}
