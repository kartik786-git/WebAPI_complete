using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Specifications;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationPatternController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecificationPatternController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetProductsbyNamespefec")]
        public async Task<IActionResult> GetProductsbyNamespecification(string name)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new ProductByNamespec(name);
            var results = await productRepository.FindAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsbyIdpecification")]
        public async Task<IActionResult> GetProductsbyIdpecification(int id)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new ProductByIdspec(id);
            var results = await productRepository.FindAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsbyNameOrderByspec")]
        public async Task<IActionResult> GetProductsbyNameOrderByspec(string search)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new Prodctsbynameorderbyspec(search);
            var results = await productRepository.GetAllAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsbyNameOrderByDescspec")]
        public async Task<IActionResult> GetProductsbyNameOrderByDescspec(string search)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new Productsbynameorderbydescsepc(search);
            var results = await productRepository.GetAllAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsbyNameOrderBypagingsepc")]
        public async Task<IActionResult> GetProductsbyNameOrderBypaingspec(string search, int pageNumber, int pageSize)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new Productsbynameorderbypagingsepc(search,pageNumber,pageSize);
            var results = await productRepository.GetAllAsync(spec);
            return Ok(results);
        }
    }
}
