using Microsoft.EntityFrameworkCore;
using ECommerceApp.Core.Entities;

namespace ECommerceApp.Repository.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<User>()
                .HasOne(user => user.Address)
                .WithOne(address => address.User)
                .HasForeignKey<Address>(address => address.UserId);

           
            modelBuilder.Entity<User>()
                .HasMany(user => user.Orders)
                .WithOne(order => order.User)
                .HasForeignKey(order => order.UserId);

           
            modelBuilder.Entity<Order>()
                .HasMany(order => order.Items)
                .WithOne(orderItem => orderItem.Order)
                .HasForeignKey(orderItem => orderItem.OrderId);

        
            modelBuilder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Product)
                .WithMany(product => product.OrderItems)
                .HasForeignKey(orderItem => orderItem.ProductId);
        }
    }
}