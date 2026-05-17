namespace CosmeticEnterpriseBack.Api.Extensions;

public static class CorsExtensions 
{
    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontendCorsPolicy", policy =>
            {
                policy
                    .WithOrigins("http://localhost:5173","https://localhost:5173")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}