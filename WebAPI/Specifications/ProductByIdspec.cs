using WebAPI.Entity;

namespace WebAPI.Specifications
{
    public class ProductByIdspec : BaseSpecification<Product>
    {
        public ProductByIdspec(int id): base(x => x.ProductId == id)
        {
            
        }
    }
}
