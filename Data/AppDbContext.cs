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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
    // {
    //     string notInDockComp =
    //         "Host=localhost;Port=5434;Database=cosmeticenterprise;Username=postgres;Password=postgres;";
    //     //string inDockComp = "Host=pg;Port=5432;Database=postgres;Username=postgres;Password=example;";
    //     optBuilder.UseNpgsql(notInDockComp);
    //     // options => options.EnableRetryOnFailure(
    //     //     maxRetryCount: 5,
    //     //     maxRetryDelay: TimeSpan.FromSeconds(10),
    //     //     errorCodesToAdd: null));
    //     optBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
    //         builder.AddConsole().AddFilter(level => level >= LogLevel.Information)));
    //     //optBuilder.UseLoggerFactory(Logger)
    //     //optBuilder.LogTo(Console.WriteLine);
    //
    // }
}