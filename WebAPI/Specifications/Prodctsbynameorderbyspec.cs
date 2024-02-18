using WebAPI.Entity;

namespace WebAPI.Specifications
{
    public class Prodctsbynameorderbyspec : BaseSpecification<Product>
    {
        public Prodctsbynameorderbyspec(string name) 
            : base(x => x.ProductName.Contains(name))
        {
            ApplyOrderBy(x => x.ProductName);
            //ApplyPaging(1, 2);
            //AddInclude(x => x.Orders);
        }
    }
}
