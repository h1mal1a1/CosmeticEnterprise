using CosmeticEnterpriseBack.Api.Extensions;
using CosmeticEnterpriseBack.Infrastructure.Extensions;
using CosmeticEnterpriseBack.Api.Middleware;
using Minio;

namespace CosmeticEnterpriseBack;

public static class Launcher
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddDatabase();
        builder.AddCors();
        builder.AddMinio();
        builder.AddAuthenticationAndAuthorization();

        builder.Services.AddApplicationServices();
        builder.Services.AddApiServices();

        builder.AddSwagger();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        
        var app = builder.Build();

        app.ApplyMigrations();
        await app.ApplySeedDataAsync();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandlingMiddleware();
        app.UseHttpsRedirection();
        app.UseCors("FrontendCorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}