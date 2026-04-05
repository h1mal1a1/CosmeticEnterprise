using CosmeticEnterpriseBack.Data;
using Microsoft.EntityFrameworkCore;

namespace CosmeticEnterpriseBack.Extensions;

public static class MigrationExtensions
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("MigrationExtensions");
        const int maxRetries = 10;
        var delay = TimeSpan.FromSeconds(3);
        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                logger.LogInformation("Applying migrations. Attempt {Attempt}/{MaxRetries}", attempt, maxRetries);
                dbContext.Database.Migrate();
                logger.LogInformation("Migrations applied successfully");
                return app;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Migration attempt {Attempt}/{MaxRetries} failed", attempt, maxRetries);
                if (attempt == maxRetries)
                    throw;
                Thread.Sleep(delay);
            }
        }

        return app;
    }
}