using ECommerceApp.Core.Interfaces.Repositories;
using ECommerceApp.Core.Interfaces.Services;
using ECommerceApp.Core.Entities;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace ECommerceApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Address> _addressRepository;

        public UserService(IRepository<User> userRepository, IRepository<Address> addressRepository)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        public async Task AddUserAsync()
        {
            try
            {
                Console.Write("Name: ");
                string userName = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(userName))
                {
                    Console.WriteLine("User name cannot be empty!");
                    return;
                }

                Console.Write("Email: ");
                string userEmail = Console.ReadLine() ?? "" ;

                if (string.IsNullOrWhiteSpace(userEmail) || !userEmail.Contains("@"))
                {
                    Console.WriteLine("Invalid email format!");
                    return;
                }

                var newUser = new User
                {
                    Name = userName,
                    Email = userEmail
                };

                await _userRepository.AddAsync(newUser);

                Console.Write("City: ");
                string addressCity = Console.ReadLine() ?? "";

                Console.Write("Street: ");
                string addressStreet = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(addressCity) || string.IsNullOrWhiteSpace(addressStreet))
                {
                    Console.WriteLine("Address city and street cannot be empty!");
                    return;
                }

                var newAddress = new Address
                {
                    UserId = newUser.Id,
                    City = addressCity,
                    Street = addressStreet
                };

                await _addressRepository.AddAsync(newAddress);
                Console.WriteLine("User Added Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task DeleteUserAsync()
        {
            try
            {
                Console.Write("Enter User ID: ");
                string userIdInput = Console.ReadLine() ?? "";

                if (!Guid.TryParse(userIdInput, out Guid userId))
                {
                    Console.WriteLine("Invalid User ID format!");
                    return;
                }

                await _userRepository.DeleteAsync(userId);
                Console.WriteLine("User Deleted Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
            }
        }

        public async Task UpdateUserAsync()
        {
            try
            {
                Console.Write("Enter User ID: ");
                string userIdInput = Console.ReadLine() ?? "";

                if (!Guid.TryParse(userIdInput, out Guid userId))
                {
                    Console.WriteLine("Invalid User ID format!");
                    return;
                }

                var userToUpdate = await _userRepository.GetByIdAsync(userId);
                if (userToUpdate == null)
                {
                    Console.WriteLine($"User with ID '{userId}' not found!");
                    return;
                }

                Console.Write("New Name: ");
                string newName = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("User name cannot be empty!");
                    return;
                }

                Console.Write("New Email: ");
                string newEmail = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
                {
                    Console.WriteLine("Invalid email format!");
                    return;
                }

                userToUpdate.Name = newName;
                userToUpdate.Email = newEmail;

                await _userRepository.UpdateAsync(userToUpdate);
                Console.WriteLine("User Updated Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
            }
        }
    }
}