using CosmeticEnterpriseBack.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Customers> Customers { get; set; }
    public DbSet<SalesChannels> SalesChannels { get; set; }
    public DbSet<OrderStatuses> OrderStatuses { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrderItems> OrderItems { get; set; }
    public DbSet<FinishedProducts> FinishedProducts { get; set; }
    public DbSet<Recipes> Recipes { get; set; }
    public DbSet<ProductCategories> ProductCategories { get; set; }
    public DbSet<ProductParties> ProductParties { get; set; }
    public DbSet<LeftoversInWarehouses> LeftoversInWarehouses { get; set; }
    public DbSet<Warehouses> Warehouses { get; set; }
    public DbSet<UnitsOfMeasurement> UnitsOfMeasurements { get; set; }
    public DbSet<Materials> Materials { get; set; }
    public DbSet<SupplyPositions> SupplyPositions { get; set; }
    public DbSet<SuppliesFromSuppliers> SuppliesFromSuppliers { get; set; }
    public DbSet<Suppliers> Suppliers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    
    public DbSet<FinishedProductImages> FinishedProductImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}