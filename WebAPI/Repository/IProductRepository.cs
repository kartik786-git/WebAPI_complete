using WebAPI.Common;
using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByName(string productName);
        Task<PaginatedList<Product>> GetAllProuctsWithPagging(int page, int pageSize, string searchTerm);
        Task<Product> GetProuductsByProductId(int productId);
    }
}
