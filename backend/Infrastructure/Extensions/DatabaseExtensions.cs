using CosmeticEnterpriseBack.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Infrastructure.Extensions;

public static class DatabaseExtensions 
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")));
    }
}