using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Models;
using System;
using System.Threading.Tasks;

namespace ECommerceApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repo;

        public ProductService(IRepository<Product> repo)
        {
            _repo = repo;
        }

        public async Task AddProduct()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Product name cannot be empty!");
                return;
            }

            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Invalid price format! Please enter a valid positive number.");
                return;
            }

            var product = new Product
            {
                Name = name,
                Price = price
            };

            await _repo.AddAsync(product);
            Console.WriteLine("Product Added!");
        }

        public async Task ViewProducts()
        {
            var products = await _repo.GetAllAsync();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            foreach (var p in products)
                Console.WriteLine($"{p.Id} - {p.Name} - ${p.Price}");
        }

        public async Task DeleteProduct()
        {
            Console.Write("Enter ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format!");
                return;
            }

            await _repo.DeleteAsync(id);
            Console.WriteLine("Product Deleted!");
        }
    }
}