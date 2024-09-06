namespace WebApplication3.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class ProductListResponse
    {
        public List<Product> Products { get; set; }
    }

    public class CaptchaResponse
    {
        public bool Success { get; set; }
        public double Score { get; set; }
    }
}

