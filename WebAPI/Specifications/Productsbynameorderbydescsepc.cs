using WebAPI.Data;
using WebAPI.Entity;

namespace WebAPI.Specifications
{
    public class Productsbynameorderbydescsepc : BaseSpecification<Product>
    {
        public Productsbynameorderbydescsepc(string name) 
            :base(x => x.ProductName.Contains(name))
        {
            ApplyOrderByDescending(x => x.ProductName);
        }
    }
}
