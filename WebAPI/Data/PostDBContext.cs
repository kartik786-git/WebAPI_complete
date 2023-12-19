using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Data
{
    public class PostDBContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public PostDBContext(DbContextOptions<PostDBContext> dbContextOptions):
            base(dbContextOptions)
        {
            
        }
    }
}
