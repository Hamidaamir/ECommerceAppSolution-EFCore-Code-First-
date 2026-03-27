using System.Threading.Tasks;

namespace ECommerceApp.Core.Interfaces.IServices
{
    public interface IOrderService
    {
        Task CreateOrder();
        Task ViewOrders();
    }
}