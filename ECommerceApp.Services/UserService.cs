using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

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
            EnsureDirectoryExists();

            var users = await _userRepo.GetAllAsync();
            var addresses = await _addressRepo.GetAllAsync();

            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            var email = Console.ReadLine() ?? "";

            var user = new User { Id = users.Count + 1, Name = name, Email = email };
            users.Add(user);

            Console.Write("City: ");
            var city = Console.ReadLine() ?? "";
            Console.Write("Street: ");
            var street = Console.ReadLine() ?? "";

            var address = new Address { Id = addresses.Count + 1, UserId = user.Id, City = city, Street = street };
            addresses.Add(address);

            await _userRepo.SaveAllAsync(users);
            await _addressRepo.SaveAllAsync(addresses);

            Console.WriteLine("User + Address Added!");
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
            EnsureDirectoryExists();

            var users = await _userRepo.GetAllAsync();
            var addresses = await _addressRepo.GetAllAsync();

            Console.Write("Enter User ID: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            users.RemoveAll(u => u.Id == id);
            addresses.RemoveAll(a => a.UserId == id);

            await _userRepo.SaveAllAsync(users);
            await _addressRepo.SaveAllAsync(addresses);

            Console.WriteLine("User deleted!");
        }

        public async Task UpdateUser()
        {
            EnsureDirectoryExists();
            var users = await _userRepo.GetAllAsync();

            Console.Write("Enter User ID: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) { Console.WriteLine("User not found!"); return; }

            Console.Write("New Name: ");
            user.Name = Console.ReadLine() ?? "";
            Console.Write("New Email: ");
            user.Email = Console.ReadLine() ?? "";

            await _userRepo.SaveAllAsync(users);
            Console.WriteLine("User updated!");
        }

        private void EnsureDirectoryExists()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Files");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}