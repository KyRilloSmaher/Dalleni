namespace Dalleni.Domin.Interfaces.Repositories
{
    public interface IUnitOfWork<TUser> : IDisposable where TUser : class
    {
        IApplicationUserRepository Users { get; }
        IQuestionRepository Questions { get; }
        IAnswerRepository Answers { get; }
        ICommentRepository Comments { get; }
        ICategoryRepository Categories { get; }
        ITagRepository Tags { get; }
        IQuestionTagRepository QuestionTags { get; }
        IOfficialEntityRepository OfficialEntities { get; }
        IServiceRepository Services { get; }
        IVoteRepository Votes { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IOtpCodeRepository OtpCodes { get; }
        ISavedQuestionsRepository SavedQuestionsRepository { get; }
        IExternalLoginRepository ExternalLogins { get; }
        IUserManager<TUser> UserManager { get; }
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        void DetachAllEntities();
        void DetachEntity<T>(T entity) where T : class;
        void AttachEntity<T>(T entity) where T : class;
        Task ReloadEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task CloseConnectionAsync();
    }
}
