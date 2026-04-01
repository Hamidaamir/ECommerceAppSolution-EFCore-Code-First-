using ECommerceApp.Core.Interfaces;
using ECommerceApp.Core.Interfaces.IServices;
using ECommerceApp.Core.Models;
using ECommerceApp.Repository;
using ECommerceApp.Repository.Data;
using ECommerceApp.Services;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=.\\SQLEXPRESS;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        var context = new AppDbContext(options);


        // Repositories
        IRepository<User> userRepo = new EfRepository<User>(context);
        IRepository<Address> addressRepo = new EfRepository<Address>(context);
        IRepository<Product> productRepo = new EfRepository<Product>(context);
        IRepository<Order> orderRepo = new EfRepository<Order>(context);
        IRepository<OrderItem> itemRepo = new EfRepository<OrderItem>(context);

        // Services
        IUserService userService = new UserService(userRepo, addressRepo);
        IProductService productService = new ProductService(productRepo);
        IOrderService orderService = new OrderService(orderRepo, itemRepo, productRepo);

        Console.WriteLine("DB Connected");

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