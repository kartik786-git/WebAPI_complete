using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Data
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public BlogDbContext(DbContextOptions<BlogDbContext> dbContextOptions) : 
            base(dbContextOptions)
        {
            
        }
    }
}
