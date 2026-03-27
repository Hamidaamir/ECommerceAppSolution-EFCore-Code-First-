using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace ECommerceApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<OrderItem> _itemRepo;
        private readonly IRepository<Product> _productRepo;

        public OrderService(IRepository<Order> orderRepo, IRepository<OrderItem> itemRepo, IRepository<Product> productRepo)
        {
            _orderRepo = orderRepo;
            _itemRepo = itemRepo;
            _productRepo = productRepo;
        }

        public async Task CreateOrder()
        {
            EnsureDirectoryExists();

            var orders = await _orderRepo.GetAllAsync();
            var items = await _itemRepo.GetAllAsync();
            var products = await _productRepo.GetAllAsync();

            Console.Write("Enter UserId: ");
            int userId = int.Parse(Console.ReadLine() ?? "0");

            var order = new Order { Id = orders.Count + 1, UserId = userId, OrderDate = DateTime.Now };
            orders.Add(order);

            while (true)
            {
                Console.Write("Enter ProductId (0 to stop): ");
                int pid = int.Parse(Console.ReadLine() ?? "0");
                if (pid == 0) break;

                Console.Write("Quantity: ");
                int qty = int.Parse(Console.ReadLine() ?? "0");

                items.Add(new OrderItem { Id = items.Count + 1, OrderId = order.Id, ProductId = pid, Quantity = qty });
            }

            await _orderRepo.SaveAllAsync(orders);
            await _itemRepo.SaveAllAsync(items);
            Console.WriteLine("Order Created!");
        }

        public async Task ViewOrders()
        {
            var orders = await _orderRepo.GetAllAsync();
            var items = await _itemRepo.GetAllAsync();
            var products = await _productRepo.GetAllAsync();

            foreach (var order in orders)
            {
                var total = items
                    .Where(i => i.OrderId == order.Id)
                    .Join(products, i => i.ProductId, p => p.Id, (i, p) => i.Quantity * p.Price)
                    .Sum();

                Console.WriteLine($"Order {order.Id} | Total: {total}");
            }
        }

        private void EnsureDirectoryExists()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Files");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}