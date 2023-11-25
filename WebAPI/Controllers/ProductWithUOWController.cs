using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.VeiwModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithUOWController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductWithUOWController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var restul = await _unitOfWork.GetRepository<Product>().GetAllAsync();

            return Ok(restul);

        }

        [HttpGet("productbyname")]
        public async Task<IActionResult> GetByName(string proudctName)
        {
            var product = await _unitOfWork.ProductRepository.GetProductsByName(proudctName);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductRequest product)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransactionAsync();

                var productEnitity = new Product
                {
                    Price = product.Price,
                    ProductName = product.ProductName
                };

                var productrestul = await _unitOfWork.GetRepository<Product>().AddAsync(productEnitity);

                await _unitOfWork.SaveChangesAsync();

                var orderEntity = new Order
                {
                    OrderDate = DateTime.Now,
                    ProductId = productrestul.ProductId
                };

                await _unitOfWork.GetRepository<Order>().AddAsync(orderEntity);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return StatusCode((int)HttpStatusCode.Created, new { Id = productrestul.ProductId });
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
