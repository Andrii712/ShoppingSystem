using Microsoft.EntityFrameworkCore;

using ShoppingSystem.Models.Entity;

namespace ShoppingSystem.Data
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<SupermarketModel> Supermarkets { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetailsModel> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<CustomerModel>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasKey(customer => customer.ID)
                    .HasName("PK_CustomerId");
                entity.HasIndex(customer => customer.FirstName)
                    .HasName("Index_CustomerFName");
                entity.HasIndex(customer => customer.LastName)
                    .HasName("Index_CustomerLName");
                entity.HasIndex(customer => new { customer.LastName, customer.FirstName })
                    .HasName("Index_CustomerFullName");
            });

            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(product => product.ID)
                    .HasName("PK_ProductId");
                entity.HasIndex(product => product.Name)
                    .HasName("Index_ProductName")
                    .IsUnique();
            });

            modelBuilder.Entity<SupermarketModel>(entity =>
            {
                entity.ToTable("SuperMarkets");
                entity.HasKey(supermarket => supermarket.ID)
                    .HasName("PK_SuperMarketId");
                entity.HasIndex(supermarket => supermarket.Name)
                    .HasName("Index_SuperMarketName");
            });

            modelBuilder.Entity<OrderModel>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(order => order.Id)
                    .HasName("PK_OrderId");
                entity.HasIndex(order => order.CustomerModelId)
                    .HasName("Index_OrderCustomerId");
                entity.HasIndex(order => order.SupermarketModelId)
                    .HasName("Index_OrderSuperMarketId");
                entity.HasIndex(order => order.OrderDate)
                    .HasName("Index_OrderDate");
                entity.HasMany(order => order.OrderDetails)
                    .WithOne(details => details.Order)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderDetailsModel>(entity =>
            {
                entity.ToTable("OrderDetails");
                entity.HasKey(details => details.Id)
                    .HasName("PK_OrderDetailsId");
                entity.HasIndex(details => details.OrderId)
                    .HasName("Index_OrderId");
                entity.HasOne(details => details.Order)
                    .WithMany(order => order.OrderDetails);
            });
        }
    }
}
