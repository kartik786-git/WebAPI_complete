
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using WebAPI.Data;

namespace WebAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _myDbContext;
        private readonly BlogDbContext _blogDbContext;
        private readonly PostDBContext _postDBContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories;

        public IProductRepository ProductRepository { get; }

        private IDbContextTransaction _transaction;
        public UnitOfWork(MyDbContext myDbContext , 
            BlogDbContext blogDbContext ,
            PostDBContext postDBContext,

            IServiceProvider serviceProvider) 
        {
            _myDbContext = myDbContext;
            _blogDbContext = blogDbContext;
            _postDBContext = postDBContext;
            _serviceProvider = serviceProvider;
            _repositories = new Dictionary<Type, object>();
            ProductRepository = new ProductRepository(_myDbContext);
        }
        public async Task BeginTransactionAsync()
        {
            await _blogDbContext.Database.BeginTransactionAsync();
            await _postDBContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            //try
            //{
            //    await _transaction.CommitAsync();
            //}
            //catch
            //{
            //    await _transaction.RollbackAsync();
            //    throw;
            //}
            //finally
            //{
            //    await _transaction.DisposeAsync();
            //    _transaction = null!;
            //}

            await _blogDbContext.Database.CommitTransactionAsync();
            await _postDBContext.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _blogDbContext.Dispose();
                    _postDBContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IRepository<T>;
            }

            var repository = new Repository<T>(_myDbContext);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task RollbackAsync()
        {
            //await _transaction.RollbackAsync();
            //await _transaction.DisposeAsync();
            //_transaction = null!;
            await _blogDbContext.Database.RollbackTransactionAsync();
            await _postDBContext.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        { 
            int id = 0;
           id =  await _blogDbContext.SaveChangesAsync();
           id = await _postDBContext.SaveChangesAsync();
            return id;
            
        }

        TRepository IUnitOfWork.GetRepository<TRepository, TEntity>()
        {
            var repository = _serviceProvider.GetService<TRepository>();

            if (repository == null)
            {
                throw new InvalidOperationException($"Failed to get repository of type {typeof(TRepository)}");
            }

            // Set the DbContext
            if (repository is IRepository<TEntity> genericRepository)
            {
                genericRepository.SetDbContext(_myDbContext);
            }
            else
            {
                throw new InvalidOperationException($"Repository of type {typeof(TRepository)} does not implement IRepository<TEntity>.");
            }

            return repository;
        }

        TRepository IUnitOfWork.GetRepository<TRepository, TEntity>(string dbType = null!)
        {
            var repository = _serviceProvider.GetService<TRepository>();

            if (repository == null)
            {
                throw new InvalidOperationException($"Failed to get repository of type {typeof(TRepository)}");
            }

            // Set the DbContext
            if (repository is IRepository<TEntity> genericRepository)
            {
                switch (dbType)
                {
                    case "blog":
                        genericRepository.SetDbContext(_blogDbContext);
                        break;
                    case "post":
                        genericRepository.SetDbContext(_postDBContext);
                        break;
                    default:
                        throw new InvalidOperationException();
                        break;
                }
                genericRepository.SetDbContext(_myDbContext);
            }
            else
            {
                throw new InvalidOperationException($"Repository of type {typeof(TRepository)} does not implement IRepository<TEntity>.");
            }

            return repository;
        }
    }
}
