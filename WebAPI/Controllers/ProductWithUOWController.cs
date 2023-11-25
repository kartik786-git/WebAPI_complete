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
    public class ProductWithUOWController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var restul = await unitOfWork.GetRepository<Product>().GetAllAsync();

            return Ok(restul);

        }

        [HttpGet("productbyname")]
        public async Task<IActionResult> GetByName(string proudctName)
        {
            var product = await unitOfWork.ProductRepository.GetProductsByName(proudctName);

            return Ok(product);
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

                var productrestul = await unitOfWork.GetRepository<Product>().AddAsync(productEnitity);

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
            catch (Exception)
            {
                await unitOfWork.RollbackAsync();
                throw;
            }
        d}
    }
}
