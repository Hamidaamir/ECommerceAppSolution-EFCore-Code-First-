namespace ECommerceApp.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task AddUserAsync();
        Task ViewUsersAsync();
        Task DeleteUserAsync();
        Task UpdateUserAsync();
    }
}