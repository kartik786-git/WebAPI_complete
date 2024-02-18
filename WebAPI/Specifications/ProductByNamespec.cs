using WebAPI.Entity;

namespace WebAPI.Specifications
{
    public class ProductByNamespec : BaseSpecification<Product>
    {
        public ProductByNamespec(string proudctName) 
            :base(x => x.ProductName.Contains(proudctName))
        {
            
        }
    }
}
