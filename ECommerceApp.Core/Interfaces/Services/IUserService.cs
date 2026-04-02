using ECommerceApp.Core.Entities;

namespace ECommerceApp.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task AddUserAsync();
        Task<List<User>> GetAllUsersAsync();
        Task DeleteUserAsync();
        Task UpdateUserAsync();
    }
}