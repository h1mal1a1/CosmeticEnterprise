using System.Security.Claims;
using System.Text;
using CosmeticEnterpriseBack.Configuration;
using CosmeticEnterpriseBack.Data;
using CosmeticEnterpriseBack.Extensions;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Middleware;
using CosmeticEnterpriseBack.Services.Auth;
using CosmeticEnterpriseBack.Services.Crud;
using CosmeticEnterpriseBack.Services.CurrentUser;
using CosmeticEnterpriseBack.Services.FinishedProductImages;
using CosmeticEnterpriseBack.Services.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;

namespace CosmeticEnterpriseBack;

public static class Launcher
{
    private static void AddJwt(WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetSection("JwtSettings");
        builder.Services.Configure<JwtSettings>(jwtSection);

        var jwtSettings = jwtSection.Get<JwtSettings>()
                          ?? throw new Exception("JwtSettings not found");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name
                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (!string.IsNullOrWhiteSpace(context.Token)) return Task.CompletedTask;
                        var accessToken = context.Request.Cookies[AuthCookieNames.AccessToken];
                        if (!string.IsNullOrWhiteSpace(accessToken))
                            context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
            });
        builder.Services.AddAuthorization();
    }
    private static void AddSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlPath);
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Введите JWT токен"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    private static void AddMinio(WebApplicationBuilder builder)
    {
        var objectStorageSection = builder.Configuration.GetSection("ObjectStorage");
        builder.Services.Configure<ObjectStorageSettings>(objectStorageSection);

        var objectStorageSettings = objectStorageSection.Get<ObjectStorageSettings>()
                                    ?? throw new Exception("ObjectStorage settings not found");

        builder.Services.AddSingleton<IMinioClient>(_ =>
            new MinioClient()
                .WithEndpoint(objectStorageSettings.Endpoint)
                .WithCredentials(objectStorageSettings.AccessKey, objectStorageSettings.SecretKey)
                .WithSSL(objectStorageSettings.UseSsl)
                .Build());

        builder.Services.AddScoped<IObjectStorageService, MinioObjectStorageService>();
    }
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        AddSwagger(builder);
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontendCorsPolicy", policy =>
            {
                policy
                    .WithOrigins("https://localhost:5173")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthCookieService, AuthCookieService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<DbContext, AppDbContext>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddScoped<IFinishedProductImageService, FinishedProductImageService>();
        builder.Services.AddCrudServices();
        AddJwt(builder);
        AddMinio(builder);
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