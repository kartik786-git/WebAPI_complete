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
      }
}
