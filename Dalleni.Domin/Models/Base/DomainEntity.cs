using Dalleni.Domin.DomainEvents;
using Dalleni.Domin.Exceptions;

namespace Dalleni.Domin.Models.Base
{
    /// <summary>
    /// Provides common audit fields and soft-delete behavior for domain entities.
    /// </summary>
    public abstract class DomainEntity
    {
        protected DomainEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the UTC date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// Gets the UTC date and time of the last entity update.
        /// </summary>
        public DateTime? UpdatedAt { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the entity is soft deleted.
        /// </summary>
        public bool IsDeleted { get; protected set; }

        /// <summary>
        /// Gets the UTC date and time when the entity was soft deleted.
        /// </summary>
        public DateTime? DeletedAt { get; protected set; }
        /// <summary>
        ///  
        /// </summary>
        private readonly List<IDomainEvent> _domainEvents = new();
        /// <summary>
        /// 
        /// </summary>

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Updates the audit timestamp for the entity.
        /// </summary>
        protected void MarkUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Throws a domain exception when the entity is deleted.
        /// </summary>
        protected void EnsureNotDeleted()
        {
            if (IsDeleted)
            {
                throw new DomainException("The entity is deleted and cannot be modified.");
            }
        }

        /// <summary>
        /// Soft deletes the entity.
        /// </summary>
        public virtual void Delete()
        {
            if (IsDeleted)
            {
                throw new DomainException("The entity has already been deleted.");
            }

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            MarkUpdated();
        }

        /// <summary>
        /// Restores a soft deleted entity.
        /// </summary>
        public virtual void Restore()
        {
            if (!IsDeleted)
            {
                throw new DomainException("The entity is already active.");
            }

            IsDeleted = false;
            DeletedAt = null;
            MarkUpdated();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        /// <summary>
        /// 
        /// </summary>

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
