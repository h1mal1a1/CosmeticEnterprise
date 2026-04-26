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
        var websiteChannelExists = await dbContext.SalesChannels.AnyAsync(c => c.Name == "Website");
        if (!websiteChannelExists)
        {
            dbContext.SalesChannels.Add(new SalesChannels { Name = "Website" });
            await dbContext.SaveChangesAsync();
        }

        const int targetProductsCount = 10;

        var random = new Random();

        var currentProductsCount = await dbContext.FinishedProducts.CountAsync();

        if (currentProductsCount < targetProductsCount)
        {
            var creamCategory = await dbContext.ProductCategories
                .FirstOrDefaultAsync(x => x.Name == "Крема");

            if (creamCategory is null)
            {
                creamCategory = new ProductCategories { Name = "Крема" };
                dbContext.ProductCategories.Add(creamCategory);
            }

            var shampooCategory = await dbContext.ProductCategories
                .FirstOrDefaultAsync(x => x.Name == "Шампуни");

            if (shampooCategory is null)
            {
                shampooCategory = new ProductCategories { Name = "Шампуни" };
                dbContext.ProductCategories.Add(shampooCategory);
            }

            var recipe = await dbContext.Recipes
                .FirstOrDefaultAsync(x => x.Name == "Базовая рецептура");

            if (recipe is null)
            {
                recipe = new Recipes { Name = "Базовая рецептура" };
                dbContext.Recipes.Add(recipe);
            }

            var unit = await dbContext.UnitsOfMeasurements
                .FirstOrDefaultAsync(x => x.Name == "шт");

            if (unit is null)
            {
                unit = new UnitsOfMeasurement { Name = "шт" };
                dbContext.UnitsOfMeasurements.Add(unit);
            }

            await dbContext.SaveChangesAsync();

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

        var warehouse = await dbContext.Warehouses.FirstOrDefaultAsync();

        if (warehouse is null)
        {
            warehouse = new Warehouses();
            dbContext.Warehouses.Add(warehouse);
            await dbContext.SaveChangesAsync();
        }

        var productIdsWithLeftovers = await dbContext.LeftoversInWarehouses
            .Select(x => x.IdFinishedProduct)
            .ToHashSetAsync();

        var productsWithoutLeftovers = await dbContext.FinishedProducts
            .Where(x => !productIdsWithLeftovers.Contains(x.Id))
            .ToListAsync();

        if (productsWithoutLeftovers.Count == 0)
            return;

        var leftovers = productsWithoutLeftovers.Select((product, index) => new LeftoversInWarehouses
        {
            IdFinishedProduct = product.Id,
            IdWarehouse = warehouse.Id,
            Quantity = index == 0 ? 0 : random.Next(1, 11),
            ReservedQuantity = 0
        });

        dbContext.LeftoversInWarehouses.AddRange(leftovers);

        await dbContext.SaveChangesAsync();
    }
}