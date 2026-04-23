namespace Dalleni.Domin.Models.Base
{
    /// <summary>
    /// Exposes soft-delete behavior for entities.
    /// </summary>
    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; }
        DateTime? DeletedAt { get; }
        void Delete();
        void Restore();
    }
}
