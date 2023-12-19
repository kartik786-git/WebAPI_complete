using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.VeiwModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultipleDBContextController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MultipleDBContextController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("blog")]
        public async Task<IActionResult> GetBlog()
        {
            var blogRep = _unitOfWork.GetRepository<IBlogRepository, Blog>("blog");
          var result = await blogRep.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("post")]
        public async Task<IActionResult> GetPost()
        {
           var postRepo = _unitOfWork.GetRepository<IPostRepository, Post>("post");
            var result = await postRepo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("blogwithpost")]
        public async Task<IActionResult> BlogWithPost()
        {
            var blogRep = _unitOfWork.GetRepository<IBlogRepository, Blog>("blog");
            var blogresult = await blogRep.GetAllAsync();

            var postRepo = _unitOfWork.GetRepository<IPostRepository, Post>("post");
            var postresult = await postRepo.GetAllAsync();

            var blogs = blogresult.GroupJoin(postresult,
                blog => blog.Id, post => post.BlogId, (blog, post) => new BlogResponse
                {
                    Id = blog.Id,
                    Description = blog.Description,
                    Name = blog.Name,
                    PostList = _mapper.Map<List<PostResponse>>(post)
                }).ToList();

            return Ok(blogs);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BlogPostReqeust blogPostReqeust)
        {
            try
            {
                using var tran = _unitOfWork.BeginTransactionAsync();

                var blogRep = _unitOfWork.GetRepository<IBlogRepository, Blog>("blog");
                var blogEnity = _mapper.Map<Blog>(blogPostReqeust);
                var blogResult = await blogRep.AddAsync(blogEnity);
                await _unitOfWork.SaveChangesAsync();

                var postRepo = _unitOfWork.GetRepository<IPostRepository, Post>("post");
                var postEnity = _mapper.Map<List<Post>>(blogPostReqeust.PostRequests);
                postEnity.ForEach(x =>
                {
                    x.BlogId = blogResult.Id;
                });
                await postRepo.AddRangeAsync(postEnity);
                await _unitOfWork.SaveChangesAsync();

                //throw new Exception("transcaiotn exapiont ");
                await _unitOfWork.CommitAsync();
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
               return StatusCode(500,new {errro = ex.Message});
            }
            
        }
    }
}
