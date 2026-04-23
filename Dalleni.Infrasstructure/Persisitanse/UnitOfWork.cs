using Dalleni.Application.Commans;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Infrastructure.Persisitanse.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dalleni.Infrastructure.Persisitanse
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private IDbContextTransaction? _currentTransaction;
        private readonly IDomainEventDispatcher _dispatcher;
        private bool _disposed;
        private int _transactionCount;

        public UnitOfWork(
            ApplicationDbContext context,
            IApplicationUserRepository users,
            IQuestionRepository questions,
            IAnswerRepository answers,
            ICommentRepository comments,
            ICategoryRepository categories,
            ITagRepository tags,
            IQuestionTagRepository questionTags,
            IOfficialEntityRepository officialEntities,
            IServiceRepository services,
            IVoteRepository votes,
            IRefreshTokenRepository refreshTokens,
            IOtpCodeRepository otpCodes,
            IExternalLoginRepository externalLogins,
            IUserManager<ApplicationUser> userManager,
            ISavedQuestionsRepository savedQuestionsRepository,
            IDomainEventDispatcher dispatcher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Users = users;
            Questions = questions;
            Answers = answers;
            Comments = comments;
            Categories = categories;
            Tags = tags;
            QuestionTags = questionTags;
            OfficialEntities = officialEntities;
            Services = services;
            Votes = votes;
            RefreshTokens = refreshTokens;
            OtpCodes = otpCodes;
            ExternalLogins = externalLogins;
            UserManager = userManager;
            SavedQuestionsRepository = savedQuestionsRepository;
            _dispatcher = dispatcher;
        }

        public IApplicationUserRepository Users { get; }
        public IQuestionRepository Questions { get; }
        public IAnswerRepository Answers { get; }
        public ICommentRepository Comments { get; }
        public ICategoryRepository Categories { get; }
        public ITagRepository Tags { get; }
        public IQuestionTagRepository QuestionTags { get; }
        public IOfficialEntityRepository OfficialEntities { get; }
        public IServiceRepository Services { get; }
        public IVoteRepository Votes { get; }
        public ISavedQuestionsRepository SavedQuestionsRepository { get; }
        public IRefreshTokenRepository RefreshTokens { get; }
        public IOtpCodeRepository OtpCodes { get; }
        public IExternalLoginRepository ExternalLogins { get; }
        public IUserManager<ApplicationUser> UserManager { get; }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (_repositories.TryGetValue(type, out var repository))
            {
                return (IRepository<T>)repository;
            }

            var createdRepository = new Repository<T>(_context);
            _repositories[type] = createdRepository;
            return createdRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is not null)
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await _context.SaveChangesAsync(cancellationToken);
                await _dispatcher.DispatchAsync(_context);
                await transaction.CommitAsync(cancellationToken);
                DetachAllEntities();
                return result;
            }
            catch
            {
                DetachAllEntities();
                throw;
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is not null)
            {
                _transactionCount++;
                return;
            }

            _transactionCount = 1;
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is null)
            {
                throw new InvalidOperationException("No active transaction to commit.");
            }

            _transactionCount--;
            if (_transactionCount > 0)
            {
                return;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
                DetachAllEntities();
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction is not null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction is null)
            {
                return;
            }

            try
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                DetachAllEntities();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
                _transactionCount = 0;
            }
        }

        public void DetachAllEntities()
        {
            foreach (var entry in _context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }

        public void DetachEntity<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void AttachEntity<T>(T entity) where T : class
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
            }
        }

        public Task ReloadEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            return _context.Entry(entity).ReloadAsync(cancellationToken);
        }

        public async Task CloseConnectionAsync()
        {
            DetachAllEntities();
            await _context.Database.CloseConnectionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
            {
                return;
            }

            if (_currentTransaction is not null)
            {
                _currentTransaction.Rollback();
                _currentTransaction.Dispose();
            }

            DetachAllEntities();
            _context.Dispose();
            _repositories.Clear();
            _disposed = true;
        }
    }
}
