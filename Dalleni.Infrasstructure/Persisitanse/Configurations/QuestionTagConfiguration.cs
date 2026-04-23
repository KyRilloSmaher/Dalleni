using Dalleni.Domin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dalleni.Infrastructure.Persisitanse.Configurations
{
    public class QuestionTagConfiguration : IEntityTypeConfiguration<QuestionTag>
    {
        public void Configure(EntityTypeBuilder<QuestionTag> builder)
        {
            builder.HasKey(x => new { x.QuestionId, x.TagId });
            builder.HasOne(x => x.Question).WithMany(x => x.QuestionTags).HasForeignKey(x => x.QuestionId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Tag).WithMany(x => x.QuestionTags).HasForeignKey(x => x.TagId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
