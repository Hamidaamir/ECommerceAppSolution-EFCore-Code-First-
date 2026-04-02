using ECommerceApp.Core.Interfaces.Repositories;
using ECommerceApp.Core.Interfaces.Services;
using ECommerceApp.Core.Entities;
using ECommerceApp.Repository;
using ECommerceApp.Repository.Data;
using ECommerceApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
                        case "2": await userService.ViewUsersAsync(); break;
                        case "3": await userService.DeleteUserAsync(); break;
                        case "4": await userService.UpdateUserAsync(); break;
                        case "5": await productService.AddProductAsync(); break;
                        case "6": await productService.ViewProductsAsync(); break;
                        case "7": await productService.DeleteProductAsync(); break;
                        case "8": await orderService.CreateOrderAsync(); break;
                        case "9": await orderService.ViewOrdersAsync(); break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine(" Invalid option! Please select a valid menu option.");
                            break;
                    }
                }
                catch (Exception menuEx)
                {
                    Console.WriteLine($" Menu Error: {menuEx.Message}");
                }
            }
        }
        catch (FileNotFoundException fileEx)
        {
            Console.WriteLine($" Configuration Error: {fileEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Critical Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}