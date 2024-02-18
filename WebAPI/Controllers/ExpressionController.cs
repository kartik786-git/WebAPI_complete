using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpressionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpressionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("serachincludeorderPagination")]
        public async Task<IActionResult> GetProductWithPagination(
            string? name = null,
            int? pageNumber = null,
            int? pageSize = null)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var results = await productRepository.SearchOrderAndPaginateAsync
                (!string.IsNullOrWhiteSpace(name) ? x => x.ProductName.Contains(name) : null, x => x.ProductId, false, pageNumber, pageSize);
            return Ok(new { results.Result, results.TotalNumber, results.TotalPages, results.IsNext, results.IsPrevious });
        }

        [HttpGet("GetProductbyNamePrice")]
        public async Task<IActionResult> GetProductbyNamePrice(string name, decimal price)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await productRepository.FindAsync(x => x.ProductName == name && x.Price == price);
            return Ok(results);
        }

        [HttpGet("GetProductsbyName")]
        public async Task<IActionResult> GetProductsbyName(string name)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await productRepository.FindAllAsync(x => x.ProductName.Contains(name));
            return Ok(results);
        }

        [HttpGet("GetProductsCountwithname")]
        public async Task<IActionResult> GetProductsbyCountname(string name)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await productRepository.CountAsync(x => x.ProductName.Contains(name));
            return Ok(results);
        }

        [HttpGet("GetProductsCount")]
        public async Task<IActionResult> GetProductsbyCount()
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await productRepository.CountAsync();
            return Ok(results);
        }
    }
}
