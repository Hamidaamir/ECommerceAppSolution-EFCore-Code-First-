using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Models;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

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
            EnsureDirectoryExists();
            var products = await _repo.GetAllAsync();

            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Price: ");
            var price = decimal.Parse(Console.ReadLine() ?? "0");

            products.Add(new Product { Id = products.Count + 1, Name = name, Price = price });
            await _repo.SaveAllAsync(products);
        }

        public async Task ViewProducts()
        {
            var products = await _repo.GetAllAsync();
            foreach (var p in products)
                Console.WriteLine($"{p.Id} - {p.Name} - {p.Price}");
        }

        public async Task DeleteProduct()
        {
            EnsureDirectoryExists();
            var products = await _repo.GetAllAsync();
            Console.Write("Enter ID to delete: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            products.RemoveAll(p => p.Id == id);
            await _repo.SaveAllAsync(products);
        }

        private void EnsureDirectoryExists()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Files");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}