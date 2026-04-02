using ECommerceApp.Core.Entities;

namespace ECommerceApp.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task AddProductAsync();
        Task<List<Product>> GetAllProductsAsync();
        Task DeleteProductAsync();
    }
}