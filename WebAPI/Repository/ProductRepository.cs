using Microsoft.EntityFrameworkCore;
using WebAPI.Common;
using WebAPI.Data;
using WebAPI.Entity;

namespace WebAPI.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }

        public async Task<PaginatedList<Product>> GetAllProuctsWithPagging(int page, int pageSize, string searchTerm)
        {
            IQueryable<Product> query = _dbSet
            .Include(p => p.Orders); // Include orders in the query

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => EF.Functions.Like(p.ProductName, $"%{searchTerm}%"));
            }

            var Result = await PaginatedList<Product>.
                ToPagedList(query.OrderBy(x => x.ProductId), page, pageSize);
            return Result;
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string productName)
        {
           return await _dbSet.Where(p => p.ProductName.Contains(productName)).ToListAsync();
        }

        public async Task<Product> GetProuductsByProductId(int productId)
        {
            return await _dbSet.Include(p => p.Orders).Where(x => x.ProductId == productId).FirstOrDefaultAsync();
        }
    }
}
