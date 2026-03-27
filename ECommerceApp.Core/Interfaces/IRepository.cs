using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task SaveAllAsync(List<T> data);
    }
}