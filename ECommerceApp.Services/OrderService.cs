using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            Console.Write("Enter UserId: ");
            if (!int.TryParse(Console.ReadLine(), out int userId) || userId <= 0)
            {
                Console.WriteLine("Invalid User ID format!");
                return;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now
            };

            await _orderRepo.AddAsync(order);
            var availableProducts = await _productRepo.GetAllAsync();

            while (true)
            {
                Console.Write("Enter ProductId (0 to stop): ");
                if (!int.TryParse(Console.ReadLine(), out int pid))
                {
                    Console.WriteLine("Invalid Product ID format!");
                    continue;
                }

                if (pid == 0)
                    break;

            
                var product = availableProducts.FirstOrDefault(p => p.Id == pid);
                if (product == null)
                {
                    Console.WriteLine($"Product with ID {pid} does not exist!");
                    continue;
                }

                Console.Write("Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
                {
                    Console.WriteLine("Invalid quantity format! Please enter a positive number.");
                    continue;
                }

                var item = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = pid,
                    Quantity = qty
                };

                try
                {
                    await _itemRepo.AddAsync(item);
                    Console.WriteLine($"Item added to order! Product: {product.Name}, Price: ${product.Price:F2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding item to order: {ex.Message}");
                }
            }

            Console.WriteLine("Order Created!");
        }

        public async Task ViewOrders()
        {
            var orders = await _orderRepo.GetAllAsync();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            var items = await _itemRepo.GetAllAsync();
            var products = await _productRepo.GetAllAsync();

            foreach (var order in orders)
            {
                var total = items
                    .Where(i => i.OrderId == order.Id)
                    .Join(products,
                        i => i.ProductId,
                        p => p.Id,
                        (i, p) => i.Quantity * p.Price)
                    .Sum();

                Console.WriteLine($"Order {order.Id} | User: {order.UserId} | Date: {order.OrderDate:yyyy-MM-dd} | Total: ${total:F2}");
            }
        }
    }
}