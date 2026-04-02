using ECommerceApp.Core.Interfaces.Repositories;
using ECommerceApp.Core.Interfaces.Services;
using ECommerceApp.Core.Entities;


namespace ECommerceApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProductAsync()
        {
            try
            {
                Console.Write("Product Name: ");
                string productName = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine("Product name cannot be empty!");
                    return;
                }

                Console.Write("Product Price: ");
                string priceInput = Console.ReadLine() ?? "";

                if (!decimal.TryParse(priceInput, out decimal productPrice))
                {
                    Console.WriteLine("Invalid price format! Please enter a valid number.");
                    return;
                }

                if (productPrice < 0)
                {
                    Console.WriteLine("Product price cannot be negative!");
                    return;
                }

                var newProduct = new Product
                {
                    Name = productName,
                    Price = productPrice
                };

                await _productRepository.AddAsync(newProduct);
                Console.WriteLine("Product Added Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }

        public async Task ViewProductsAsync()
        {
            try
            {
                var allProducts = await _productRepository.GetAllAsync();

                if (allProducts.Count == 0)
                {
                    Console.WriteLine("No products found.");
                    return;
                }

                foreach (var product in allProducts)
                    Console.WriteLine($"{product.Id} - {product.Name} - ${product.Price:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
            }
        }

        public async Task DeleteProductAsync()
        {
            try
            {
                Console.Write("Enter Product ID to delete: ");
                string productIdInput = Console.ReadLine() ?? "";

                if (!Guid.TryParse(productIdInput, out Guid productId))
                {
                    Console.WriteLine("Invalid Product ID format!");
                    return;
                }

                await _productRepository.DeleteAsync(productId);
                Console.WriteLine("Product Deleted Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
            }
        }
    }
}