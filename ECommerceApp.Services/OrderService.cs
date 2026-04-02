using ECommerceApp.Core.Interfaces.Repositories;
using ECommerceApp.Core.Interfaces.Services;
using ECommerceApp.Core.Entities;


namespace ECommerceApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Product> _productRepository;

        public OrderService(IRepository<Order> orderRepository, IRepository<OrderItem> orderItemRepository, IRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

        public async Task CreateOrderAsync()
        {
            try
            {
                Console.Write("Enter User ID: ");
                if (!Guid.TryParse(Console.ReadLine(), out Guid userId) || userId == Guid.Empty)
                {
                    Console.WriteLine("Invalid User ID format!");
                    return;
                }

                var newOrder = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now
                };

                await _orderRepository.AddAsync(newOrder);
                Console.WriteLine("Order created successfully!");

                var availableProducts = await _productRepository.GetAllAsync();

                while (true)
                {
                    Console.Write("Enter Product ID (leave empty to stop): ");
                    string productIdInput = Console.ReadLine() ?? "";

                    if (string.IsNullOrWhiteSpace(productIdInput))
                        break;

                    if (!Guid.TryParse(productIdInput, out Guid productId))
                    {
                        Console.WriteLine("Invalid Product ID format!");
                        continue;
                    }

                    var selectedProduct = availableProducts.FirstOrDefault(prod => prod.Id == productId);
                    if (selectedProduct == null)
                    {
                        Console.WriteLine($"Product with ID {productId} does not exist!");
                        continue;
                    }

                    Console.Write("Quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int orderItemQuantity) || orderItemQuantity <= 0)
                    {
                        Console.WriteLine("Invalid quantity! Please enter a positive number.");
                        continue;
                    }

                    var newOrderItem = new OrderItem
                    {
                        OrderId = newOrder.Id,
                        ProductId = productId,
                        Quantity = orderItemQuantity
                    };

                    await _orderItemRepository.AddAsync(newOrderItem);
                    Console.WriteLine($"Item added! Product: {selectedProduct.Name}, Price: ${selectedProduct.Price:F2}");
                }

                Console.WriteLine("Order Created Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _orderRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving orders: {ex.Message}");
                return new List<Order>();
            }
        }
    }
}