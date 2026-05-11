using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using CosmeticEnterpriseBack.Infrastructure.Configuration;
using CosmeticEnterpriseBack.Application.Constants;

namespace CosmeticEnterpriseBack.Infrastructure.Extensions;

public static class AuthExtensions 
{
    public static void AddAuthenticationAndAuthorization(this WebApplicationBuilder builder)
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
}