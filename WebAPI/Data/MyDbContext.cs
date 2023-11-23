using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> dbContextOptions) : base(dbContextOptions) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
           .HasOne(o => o.Product)
           .WithMany(p => p.Orders)
           .HasForeignKey(o => o.ProductId);
        }
    }
}
