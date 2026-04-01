namespace ECommerceApp.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }

        // Navigation property
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}