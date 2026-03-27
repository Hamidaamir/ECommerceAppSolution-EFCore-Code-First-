using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Models;
using ECommerceApp.Services;
using ECommerceApp.Repository;

class Program
{
    static async Task Main()
    {
        
        var baseDirectory = AppContext.BaseDirectory;
        var solutionRootPath = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
        var filesPath = Path.Combine(solutionRootPath, "Files");

        
        IRepository<User> userRepo = new JsonFileRepository<User>(Path.Combine(filesPath, "users.json"));
        IRepository<Address> addressRepo = new JsonFileRepository<Address>(Path.Combine(filesPath, "addresses.json"));
        IRepository<Product> productRepo = new JsonFileRepository<Product>(Path.Combine(filesPath, "products.json"));
        IRepository<Order> orderRepo = new JsonFileRepository<Order>(Path.Combine(filesPath, "orders.json"));
        IRepository<OrderItem> itemRepo = new JsonFileRepository<OrderItem>(Path.Combine(filesPath, "orderItems.json"));

        
        IUserService userService = new UserService(userRepo, addressRepo);
        IProductService productService = new ProductService(productRepo);
        IOrderService orderService = new OrderService(orderRepo, itemRepo, productRepo);

        while (true)
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

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": await userService.AddUser(); break;
                case "2": await userService.ViewUsers(); break;
                case "3": await userService.DeleteUser(); break;
                case "4": await userService.UpdateUser(); break;
                case "5": await productService.AddProduct(); break;
                case "6": await productService.ViewProducts(); break;
                case "7": await productService.DeleteProduct(); break;
                case "8": await orderService.CreateOrder(); break;
                case "9": await orderService.ViewOrders(); break;
                case "0": return;
            }
        }
    }
}