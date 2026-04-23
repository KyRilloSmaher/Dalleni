using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
    /// <summary>
    /// Represents a question tag.
    /// </summary>
    public class Tag : DomainEntity
    {
        public Tag()
        {
            QuestionTags = new List<QuestionTag>();
        }

        private Tag(string name) : this()
        {
            Id = Guid.NewGuid();
            SetName(name);
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string NormalizedName { get; private set; } = string.Empty;

        public string Slug { get; private set; } = string.Empty;

        public int UsageCount { get; private set; }

        public ICollection<QuestionTag> QuestionTags { get; private set; }

        public static Tag Create(string name)
        {
            return new Tag(name);
        }

        public void Update(string name)
        {
            EnsureNotDeleted();
            SetName(name);
            MarkUpdated();
        }

        public void IncrementUsage()
        {
            UsageCount++;
        }

        public void DecrementUsage()
        {
            if (UsageCount > 0)
                UsageCount--;
        }

        private void SetName(string name)
        {
            Name = DomainGuard.AgainstNullOrWhiteSpace(name, nameof(name));

            NormalizedName = name.Trim().ToLowerInvariant();
            Slug = NormalizedName.Replace(" ", "-");
        }
    }
}