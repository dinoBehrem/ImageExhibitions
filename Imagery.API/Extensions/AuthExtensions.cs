using Imagery.Core.Models;
using Imagery.Repository.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.API.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AuthService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<ImageryContext>().AddDefaultTokenProviders();

            var validIssuer = configuration["Jwt:Issuer"];

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = false;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    //ValidIssuer = validIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@23"))
                };
            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("SuperAdminAccess", policy => policy.RequireRole("SuperAdmin"));
                //options.AddPolicy("AdminAccess", policy => policy.RequireClaim("role", "Admin"));
                //options.AddPolicy("UserAccess", policy => policy.RequireClaim("role", "User"));
            });

            return services;
        }
    }
}
