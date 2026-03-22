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

        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Admin123";
        var userName = Environment.GetEnvironmentVariable("ADMIN_USERNAME") ?? "admin";
        var adminExists = await dbContext.Users.AnyAsync(u => u.RoleName == UserRole.Admin);
        if (adminExists) return app;
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

        return app;
    }
}