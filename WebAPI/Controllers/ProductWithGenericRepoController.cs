using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.VeiwModel;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithGenericRepoController : ControllerBase
    {
        private readonly IRepository<Product> _productRespository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _product;

        public ProductWithGenericRepoController(
            IRepository<Product> productRespository, IMapper mapper, 
            IProductRepository product)
        {
            _productRespository = productRespository;
            _mapper = mapper;
            _product = product;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productRespository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("ProuctsWithPagging")]
        public async Task<IActionResult> GetProuctsWithPagging(int page = 1, int pageSize = 10, string searchTerm = null)
        {
            var products = await _productRespository.GetAllAsync();
            var prudctdto = _mapper.Map<List<ProductRequest>>(products);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRespository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var prudctdto = _mapper.Map<ProductRequest>(product);
            return Ok(prudctdto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductRequest product)
        {

            var productentity = _mapper.Map<Product>(product);
            var createdProductReponse = await _productRespository.AddAsync(productentity);
            return CreatedAtAction(nameof(GetById), new { id = createdProductReponse.ProductId }, createdProductReponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductRequest product)
        {
            var productEntity = await _productRespository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }
            //productEntity.ProductName = product.ProductName;
            //productEntity.Price = product.Price;
            //_mapper.Map<Product>(product);
            _mapper.Map(product, productEntity);
            await _productRespository.UpdateAsync(productEntity);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRespository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRespository.DeleteAsync(product);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, 
            [FromBody] JsonPatchDocument<ProductRequest> patchProduct)
        {
            var productEntity = await _product.GetProuductsByProductId(id);
            if (productEntity == null)
            {
                return NotFound();
            }
           // productEntity.ProductName = "exitina name"
            var productRequest = _mapper.Map<ProductRequest>(productEntity);

            //ProductName = "change name"
            patchProduct.ApplyTo(productRequest, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(productRequest, productEntity);
            await _product.UpdateAsync(productEntity);
            return NoContent();
        }
    }
}
