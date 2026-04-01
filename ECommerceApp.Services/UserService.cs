using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Models;
using System;
using System.Threading.Tasks;

namespace ECommerceApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Address> _addressRepo;

        public UserService(IRepository<User> userRepo, IRepository<Address> addressRepo)
        {
            _userRepo = userRepo;
            _addressRepo = addressRepo;
        }

        public async Task AddUser()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";

            var user = new User { Name = name, Email = email };

            await _userRepo.AddAsync(user);

            Console.Write("City: ");
            var city = Console.ReadLine() ?? "";

            Console.Write("Street: ");
            var street = Console.ReadLine() ?? "";

            var address = new Address
            {
                UserId = user.Id,
                City = city,
                Street = street
            };

            await _addressRepo.AddAsync(address);

            Console.WriteLine("User Added!");
        }

        public async Task ViewUsers()
        {
            var users = await _userRepo.GetAllAsync();
            var addresses = await _addressRepo.GetAllAsync();

            foreach (var user in users)
            {
                var address = addresses.FirstOrDefault(a => a.UserId == user.Id);
                Console.WriteLine($"{user.Id} - {user.Name} - {user.Email}");
                if (address != null)
                    Console.WriteLine($"   Address: {address.City}, {address.Street}");
            }
        }

        public async Task DeleteUser()
        {
            Console.Write("Enter User ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format!");
                return;
            }

            await _userRepo.DeleteAsync(id);

            Console.WriteLine("Deleted!");
        }

        public async Task UpdateUser()
        {
            Console.Write("Enter User ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format!");
                return;
            }

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                Console.WriteLine("Not found");
                return;
            }

            Console.Write("New Name: ");
            user.Name = Console.ReadLine() ?? "";

            Console.Write("New Email: ");
            user.Email = Console.ReadLine() ?? "";

            await _userRepo.UpdateAsync(user);
            Console.WriteLine("User Updated!");
        }
    }
}