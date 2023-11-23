namespace WebAPI.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        // Foreign key
        public int ProductId { get; set; }

        // Navigation property
        public Product Product { get; set; }


    }
}
