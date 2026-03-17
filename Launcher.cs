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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        AddSwagger(builder);
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnectionString")));
        builder.Services.AddControllers();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthCookieService, AuthCookieService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<DbContext, AppDbContext>();
        builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
        builder.Services.AddCrudServices();
        AddJwt(builder);
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandlingMiddleware();
        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        app.Run();
    }
}