namespace ECommerceApp.Core.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string City { get; set; } = "";
        public string Street { get; set; } = "";
    }
}