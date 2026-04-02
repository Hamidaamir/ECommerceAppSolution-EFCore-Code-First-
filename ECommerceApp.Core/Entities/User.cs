namespace ECommerceApp.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public Address? Address { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}