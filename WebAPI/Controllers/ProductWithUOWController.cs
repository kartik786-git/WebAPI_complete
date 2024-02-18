using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.VeiwModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithUOWController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productRepository = unitOfWork.GetRepository<IProductRepository, Product>();
            var restul = await productRepository.GetAllAsync();

            return Ok(restul);

        }

        [HttpGet("ProductPagging")]
        public async Task<IActionResult> GetProductPagging(int page = 1, int pageSize = 10, string searchTerm = null)
        {
            var productRepository = unitOfWork.GetRepository<IProductRepository, Product>();
            var results = await productRepository.GetAllProuctsWithPagging(page, pageSize, searchTerm);
            var metadata = new
            {
                results.TotalCount,
                results.PageSize,
                results.CurrentPage,
                results.TotalPages,
                results.HasNext,
                results.HasPrevious,
                results
            };
            return Ok(metadata);

        }

        [HttpGet("productbyname")]
        public async Task<IActionResult> GetByName(string proudctName)
        {
            var productRepository = unitOfWork.GetRepository<IProductRepository, Product>();
           var result = await productRepository.GetProductsByName(proudctName);
            //var product = await unitOfWork.ProductRepository.GetProductsByName(proudctName);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductRequest product)
        {
            try
            {
                using var transaction = unitOfWork.BeginTransactionAsync();

                var productEnitity = new Product
                {
                    Price = product.Price,
                    ProductName = product.ProductName
                };
                var productRepository = unitOfWork.GetRepository<IProductRepository, Product>();
                var productrestul = await productRepository.AddAsync(productEnitity);

                await unitOfWork.SaveChangesAsync();

                var orderEntity = new Order
                {
                    OrderDate = DateTime.Now,
                    ProductId = productrestul.ProductId
                };

                await unitOfWork.GetRepository<Order>().AddAsync(orderEntity);
                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

                return StatusCode((int)HttpStatusCode.Created, new { Id = productrestul.ProductId });
            }
            catch (System.Exception)
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
