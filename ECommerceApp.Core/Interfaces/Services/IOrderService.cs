namespace ECommerceApp.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync();
        Task ViewOrdersAsync();
    }
}