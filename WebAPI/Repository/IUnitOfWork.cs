namespace WebAPI.Repository
{
    public interface IUnitOfWork : IDisposable
      {
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        public IProductRepository ProductRepository { get; }

        TRepository GetRepository<TRepository, TEntity>() where TRepository : class,
           IRepository<TEntity> where TEntity : class;
        TRepository GetRepository<TRepository, TEntity>(string dbType) where TRepository : class,
           IRepository<TEntity> where TEntity : class;
    }
}
