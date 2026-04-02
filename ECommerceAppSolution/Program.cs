using ECommerceApp.Core.Interfaces.Repositories;
using ECommerceApp.Core.Interfaces.Services;
using ECommerceApp.Core.Entities;
using ECommerceApp.Repository;
using ECommerceApp.Repository.Data;
using ECommerceApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

class Program
{
    static async Task Main()
    {
        try
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Connection string not found in configuration");
                return;
            }

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var dbContext = new AppDbContext(options);

            await dbContext.Database.CanConnectAsync();

            IRepository<User> userRepository = new EfRepository<User>(dbContext);
            IRepository<Address> addressRepository = new EfRepository<Address>(dbContext);
            IRepository<Product> productRepository = new EfRepository<Product>(dbContext);
            IRepository<Order> orderRepository = new EfRepository<Order>(dbContext);
            IRepository<OrderItem> orderItemRepository = new EfRepository<OrderItem>(dbContext);

            IUserService userService = new UserService(userRepository, addressRepository);
            IProductService productService = new ProductService(productRepository);
            IOrderService orderService = new OrderService(orderRepository, orderItemRepository, productRepository);

            Console.WriteLine("DB Connected Successfully!\n");

            while (true)
            {
                try
                {
                    Console.WriteLine("\n1. Add User");
                    Console.WriteLine("2. View Users");
                    Console.WriteLine("3. Delete User");
                    Console.WriteLine("4. Update User");
                    Console.WriteLine("5. Add Product");
                    Console.WriteLine("6. View Products");
                    Console.WriteLine("7. Delete Product");
                    Console.WriteLine("8. Create Order");
                    Console.WriteLine("9. View Orders");
                    Console.WriteLine("0. Exit");

                    Console.Write("Select an option: ");
                    string userChoice = Console.ReadLine() ?? "";

                    switch (userChoice)
                    {
                        case "1": await userService.AddUserAsync(); break;
                        case "2": await DisplayAllUsersAsync(userService, addressRepository); break;
                        case "3": await userService.DeleteUserAsync(); break;
                        case "4": await userService.UpdateUserAsync(); break;
                        case "5": await productService.AddProductAsync(); break;
                        case "6": await DisplayAllProductsAsync(productService); break;
                        case "7": await productService.DeleteProductAsync(); break;
                        case "8": await orderService.CreateOrderAsync(); break;
                        case "9": await DisplayAllOrdersAsync(orderService, orderItemRepository, productRepository); break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Invalid option! Please select a valid menu option.");
                            break;
                    }
                }
                catch (Exception menuEx)
                {
                    Console.WriteLine($"Menu Error: {menuEx.Message}");
                }
            }
        }
        catch (FileNotFoundException fileEx)
        {
            Console.WriteLine($"Configuration Error: {fileEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    static async Task DisplayAllUsersAsync(IUserService userService, IRepository<Address> addressRepository)
    {
        try
        {
            var allUsers = await userService.GetAllUsersAsync();
            var allAddresses = await addressRepository.GetAllAsync();

            if (allUsers.Count == 0)
            {
                Console.WriteLine("No users found.");
                return;
            }

            Console.WriteLine("\nAll Users:");
            foreach (var user in allUsers)
            {
                var userAddress = allAddresses.FirstOrDefault(addr => addr.UserId == user.Id);
                Console.WriteLine($"ID: {user.Id}");
                Console.WriteLine($"Name: {user.Name}");
                Console.WriteLine($"Email: {user.Email}");
                if (userAddress != null)
                    Console.WriteLine($"Address: {userAddress.City}, {userAddress.Street}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying users: {ex.Message}");
        }
    }

    static async Task DisplayAllProductsAsync(IProductService productService)
    {
        try
        {
            var allProducts = await productService.GetAllProductsAsync();

            if (allProducts.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("\nAll Products:");
            foreach (var product in allProducts)
            {
                Console.WriteLine($"ID: {product.Id}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: ${product.Price:F2}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying products: {ex.Message}");
        }
    }

    static async Task DisplayAllOrdersAsync(IOrderService orderService, IRepository<OrderItem> orderItemRepository, IRepository<Product> productRepository)
    {
        try
        {
            var allOrders = await orderService.GetAllOrdersAsync();

            if (allOrders.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            var allOrderItems = await orderItemRepository.GetAllAsync();
            var allProducts = await productRepository.GetAllAsync();

            Console.WriteLine("\nAll Orders:");
            foreach (var order in allOrders)
            {
                decimal orderTotalAmount = allOrderItems
                    .Where(item => item.OrderId == order.Id)
                    .Join(allProducts,
                        item => item.ProductId,
                        product => product.Id,
                        (item, product) => item.Quantity * product.Price)
                    .Sum();

                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"User ID: {order.UserId}");
                Console.WriteLine($"Date: {order.OrderDate:yyyy-MM-dd}");
                Console.WriteLine($"Total: ${orderTotalAmount:F2}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error displaying orders: {ex.Message}");
        }
    }
}