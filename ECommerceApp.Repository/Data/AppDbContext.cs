using Microsoft.EntityFrameworkCore;
using ECommerceApp.Core.Models;

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
            // One-to-One (User - Address)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            // One-to-Many (User - Orders)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            // One-to-Many (Order - OrderItems)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            // Many-to-Many via OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(i => i.ProductId);
        }
    }
}