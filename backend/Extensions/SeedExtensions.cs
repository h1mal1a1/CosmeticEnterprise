using CosmeticEnterpriseBack.Authorization;
using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.Entities;
using CosmeticEnterpriseBack.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Extensions;

public static class SeedExtensions
{
    private const int ProductsPerCategory = 5;
    private const int ProductImagesCount = 2;

    public static async Task<WebApplication> ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var objectStorageService = scope.ServiceProvider.GetRequiredService<IObjectStorageService>();

        await SeedAdminAsync(dbContext);
        await SeedAdminAddressAsync(dbContext);
        await SeedCatalogAsync(dbContext);
        await SeedProductImagesAsync(dbContext, objectStorageService);

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

    private static async Task SeedAdminAddressAsync(AppDbContext dbContext)
    {
        var admin = await dbContext.Users.FirstOrDefaultAsync(x => x.RoleName == UserRole.Admin);
        if (admin is null) return;

        var hasAddress = await dbContext.UserAddresses.AnyAsync(x => x.IdUser == admin.IdUser);
        if (hasAddress) return;

        var now = DateTime.UtcNow;

        dbContext.UserAddresses.Add(new UserAddress
        {
            IdUser = admin.IdUser,
            RecipientName = "Администратор",
            Phone = "+79999999999",
            Country = "Россия",
            City = "Москва",
            Street = "Победы",
            House = "1",
            Apartment = "1",
            PostalCode = "123123",
            Comment = "Seed адрес администратора",
            IsDefault = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        });

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

        await SeedProductsForCategoryAsync(
            dbContext,
            creamCategory.Id,
            recipe.Id,
            unit.Id,
            new[]
            {
                "Aloe Cream",
                "Hydra Cream",
                "Soft Cream",
                "Vitamin Cream",
                "Natural Cream"
            });

        await SeedProductsForCategoryAsync(
            dbContext,
            shampooCategory.Id,
            recipe.Id,
            unit.Id,
            new[]
            {
                "Aloe Shampoo",
                "Hydra Shampoo",
                "Soft Shampoo",
                "Vitamin Shampoo",
                "Natural Shampoo"
            });

        await EnsureProductPricesAsync(dbContext);
        await SeedLeftoversAsync(dbContext);
    }

    private static async Task SeedProductsForCategoryAsync(
        AppDbContext dbContext,
        long idProductCategory,
        long idRecipe,
        long idUnitsOfMeasurement,
        IReadOnlyList<string> productNames)
    {
        var random = new Random();

        foreach (var productName in productNames)
        {
            var exists = await dbContext.FinishedProducts
                .AnyAsync(x => x.Name == productName);

            if (exists)
                continue;

            dbContext.FinishedProducts.Add(new FinishedProducts
            {
                Name = productName,
                Price = random.Next(100, 1001),
                IdProductCategory = idProductCategory,
                IdRecipe = idRecipe,
                IdUnitsOfMeasurement = idUnitsOfMeasurement
            });
        }

        await dbContext.SaveChangesAsync();
    }

    private static async Task EnsureProductPricesAsync(AppDbContext dbContext)
    {
        var random = new Random();

        var productsWithInvalidPrices = await dbContext.FinishedProducts
            .Where(x => x.Price < 100 || x.Price > 1000)
            .ToListAsync();

        foreach (var product in productsWithInvalidPrices)
            product.Price = random.Next(100, 1001);

        if (productsWithInvalidPrices.Count > 0)
            await dbContext.SaveChangesAsync();
    }

    private static async Task SeedLeftoversAsync(AppDbContext dbContext)
    {
        var random = new Random();

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

        var leftovers = productsWithoutLeftovers.Select(product => new LeftoversInWarehouses
        {
            IdFinishedProduct = product.Id,
            IdWarehouse = warehouse.Id,
            Quantity = random.Next(1, 11),
            ReservedQuantity = 0
        });

        dbContext.LeftoversInWarehouses.AddRange(leftovers);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedProductImagesAsync(
        AppDbContext dbContext,
        IObjectStorageService objectStorageService)
    {
        var creams = await dbContext.FinishedProducts
            .Include(x => x.ProductCategories)
            .Include(x => x.Images)
            .Where(x => x.ProductCategories.Name == "Крема")
            .OrderBy(x => x.Id)
            .Take(ProductsPerCategory)
            .ToListAsync();

        var shampoos = await dbContext.FinishedProducts
            .Include(x => x.ProductCategories)
            .Include(x => x.Images)
            .Where(x => x.ProductCategories.Name == "Шампуни")
            .OrderBy(x => x.Id)
            .Take(ProductsPerCategory)
            .ToListAsync();

        await SeedImagesForCategoryAsync(
            dbContext,
            objectStorageService,
            creams,
            "krema");

        await SeedImagesForCategoryAsync(
            dbContext,
            objectStorageService,
            shampoos,
            "shampuni");

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedImagesForCategoryAsync(
        AppDbContext dbContext,
        IObjectStorageService objectStorageService,
        IReadOnlyList<FinishedProducts> products,
        string sourceFolderName)
    {
        for (var productIndex = 0; productIndex < products.Count; productIndex++)
        {
            var product = products[productIndex];
            var sourceVariantNumber = productIndex + 1;

            for (var imageNumber = 1; imageNumber <= ProductImagesCount; imageNumber++)
            {
                var sourcePath = Path.Combine(
                    AppContext.BaseDirectory,
                    "photos",
                    sourceFolderName,
                    sourceVariantNumber.ToString(),
                    $"{imageNumber}.jpg");

                if (!File.Exists(sourcePath))
                    continue;

                var objectKey = $"seed/products/{sourceFolderName}/{sourceVariantNumber}/{imageNumber}.jpg";

                await UploadSeedImageWithRetryAsync(
                    objectStorageService,
                    sourcePath,
                    objectKey);

                var imageExists = product.Images.Any(x => x.ObjectKey == objectKey);
                if (imageExists)
                    continue;

                if (imageNumber == 1)
                    ClearMainImage(product);

                var image = new FinishedProductImages
                {
                    IdFinishedProduct = product.Id,
                    ObjectKey = objectKey,
                    SortOrder = imageNumber - 1,
                    IsMain = imageNumber == 1
                };

                product.Images.Add(image);
                dbContext.FinishedProductImages.Add(image);
            }
        }
    }

    private static void ClearMainImage(FinishedProducts product)
    {
        foreach (var image in product.Images)
            image.IsMain = false;
    }

    private static async Task UploadSeedImageWithRetryAsync(
        IObjectStorageService objectStorageService,
        string sourcePath,
        string objectKey)
    {
        const int maxAttempts = 10;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await using var stream = File.OpenRead(sourcePath);

                await objectStorageService.UploadAsync(
                    stream,
                    objectKey,
                    "image/jpeg",
                    stream.Length);

                return;
            }
            catch when (attempt < maxAttempts)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}