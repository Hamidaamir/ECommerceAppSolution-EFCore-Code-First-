namespace ECommerceApp.Core.Entities
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
        public User? User { get; set; }
    }
}