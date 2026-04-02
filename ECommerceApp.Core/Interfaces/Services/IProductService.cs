namespace ECommerceApp.Core.Interfaces.Services
{
    public interface IProductService
    {
        Task AddProductAsync();
        Task ViewProductsAsync();
        Task DeleteProductAsync();
    }
}