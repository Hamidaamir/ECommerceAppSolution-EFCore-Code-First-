namespace ECommerceApp.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";

        // Navigation properties
        public Address? Address { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}