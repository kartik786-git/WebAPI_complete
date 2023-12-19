using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entity;

namespace WebAPI.Repository
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }
    }
}
