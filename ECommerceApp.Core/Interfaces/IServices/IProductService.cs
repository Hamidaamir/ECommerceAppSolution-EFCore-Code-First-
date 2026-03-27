using System.Threading.Tasks;

namespace ECommerceApp.Core.Interfaces.IServices
{
    public interface IProductService
    {
        Task AddProduct();
        Task ViewProducts();
        Task DeleteProduct();
    }
}