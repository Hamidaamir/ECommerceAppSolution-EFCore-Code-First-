using ECommerceApp.Core.Models;
using System.Threading.Tasks;

namespace ECommerceApp.Core.Interfaces.IServices
{
    public interface IUserService
    {
        Task AddUser();
        Task ViewUsers();
        Task DeleteUser();
        Task UpdateUser();
    }
}