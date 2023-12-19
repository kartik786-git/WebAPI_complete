using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entity;

namespace WebAPI.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(PostDBContext dbContext) : base(dbContext)
        {
        }
    }
}
