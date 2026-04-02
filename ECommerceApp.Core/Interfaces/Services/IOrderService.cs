using ECommerceApp.Core.Entities;

namespace ECommerceApp.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task CreateOrderAsync();
        Task<List<Order>> GetAllOrdersAsync();
    }
}