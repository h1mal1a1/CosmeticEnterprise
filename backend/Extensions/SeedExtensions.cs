using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Extensions;

public static class SeedExtensions
{
    public static async Task<WebApplication> ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await SeedAdminAsync(dbContext);
        await SeedCatalogAsync(dbContext);

        return app;
    }

    private static async Task SeedAdminAsync(AppDbContext dbContext)
    {
        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Admin123";
        var userName = Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "admin";

        var adminExists = await dbContext.Users.AnyAsync(u => u.RoleName == UserRole.Admin);
        if (adminExists) return;

        var passwordHasher = new PasswordHasher<User>();

        var admin = new User
        {
            Username = userName,
            PasswordHash = string.Empty,
            RoleName = UserRole.Admin
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, password);

        dbContext.Users.Add(admin);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedCatalogAsync(AppDbContext dbContext)
    {
        if (await dbContext.FinishedProducts.AnyAsync())
            return;

        var creamCategory = new ProductCategories { Name = "Крема" };
        var shampooCategory = new ProductCategories { Name = "Шампуни" };

        dbContext.ProductCategories.AddRange(creamCategory, shampooCategory);

        var recipe = new Recipes() { Name = "Базовая рецептура" };
        var unit = new UnitsOfMeasurement() { Name = "шт" };

        dbContext.Recipes.Add(recipe);
        dbContext.UnitsOfMeasurements.Add(unit);

        await dbContext.SaveChangesAsync();

        var products = new List<FinishedProducts>
        {
            new()
            {
                Name = "Nivea",
                IdProductCategory = creamCategory.Id,
                IdRecipe = recipe.Id,
                IdUnitsOfMeasurement = unit.Id
            },
            new()
            {
                Name = "Librederm",
                IdProductCategory = creamCategory.Id,
                IdRecipe = recipe.Id,
                IdUnitsOfMeasurement = unit.Id
            },
            new()
            {
                Name = "Natura Siberica",
                IdProductCategory = creamCategory.Id,
                IdRecipe = recipe.Id,
                IdUnitsOfMeasurement = unit.Id
            },

            new()
            {
                Name = "Matrix",
                IdProductCategory = shampooCategory.Id,
                IdRecipe = recipe.Id,
                IdUnitsOfMeasurement = unit.Id
            },
            new()
            {
                Name = "Kapous",
                IdProductCategory = shampooCategory.Id,
                IdRecipe = recipe.Id,
                IdUnitsOfMeasurement = unit.Id
            },
            new()
            {
                Name = "L'Oreal",
                IdProductCategory = shampooCategory.Id,
                IdRecipe = recipe.Id,
                IdUnitsOfMeasurement = unit.Id
            }
        };

        dbContext.FinishedProducts.AddRange(products);

        await dbContext.SaveChangesAsync();
    }
}