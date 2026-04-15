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
        const int targetProductsCount = 100;
        var currentProductsCount = await dbContext.FinishedProducts.CountAsync();
        if (currentProductsCount >= targetProductsCount)
            return;
        
        var creamCategory = new ProductCategories { Name = "Крема" };
        var shampooCategory = new ProductCategories { Name = "Шампуни" };

        dbContext.ProductCategories.AddRange(creamCategory, shampooCategory);

        var recipe = new Recipes() { Name = "Базовая рецептура" };
        var unit = new UnitsOfMeasurement() { Name = "шт" };

        dbContext.Recipes.Add(recipe);
        dbContext.UnitsOfMeasurements.Add(unit);

        await dbContext.SaveChangesAsync();

        var random = new Random();

        var creamNames = new[]
        {
            "Aloe Cream",
            "Hydra Cream",
            "Soft Cream",
            "Ultra Cream",
            "Bio Cream",
            "Silk Cream",
            "Vitamin Cream",
            "Fresh Cream",
            "Natural Cream",
            "Care Cream"
        };

        var shampooNames = new[]
        {
            "Aloe Shampoo",
            "Hydra Shampoo",
            "Soft Shampoo",
            "Ultra Shampoo",
            "Bio Shampoo",
            "Silk Shampoo",
            "Vitamin Shampoo",
            "Fresh Shampoo",
            "Natural Shampoo",
            "Care Shampoo"
        };

        var existingNames = await dbContext.FinishedProducts
            .Select(x => x.Name)
            .ToHashSetAsync();

        var productsToAdd = new List<FinishedProducts>();
        var missingCount = targetProductsCount - currentProductsCount;
        var sequence = currentProductsCount + 1;

        while (productsToAdd.Count < missingCount)
        {
            var isCream = random.Next(0, 2) == 0;
            var baseName = isCream
                ? creamNames[random.Next(creamNames.Length)]
                : shampooNames[random.Next(shampooNames.Length)];

            var name = $"{baseName} {sequence}";

            sequence++;

            if (existingNames.Contains(name))
                continue;

            existingNames.Add(name);

            productsToAdd.Add(new FinishedProducts
            {
                Name = name,
                IdProductCategory = isCream ? creamCategory.Id : shampooCategory.Id,
                IdRecipe = recipe.Id,
                IdUnitsOfMeasurement = unit.Id
            });
        }

        dbContext.FinishedProducts.AddRange(productsToAdd);

        await dbContext.SaveChangesAsync();
    }
}