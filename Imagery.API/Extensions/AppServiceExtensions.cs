using Imagery.Core.Models;
using Imagery.Repository.Context;
using Imagery.Repository.Repository;
using Imagery.Service.Services.Authentication;
using Imagery.Service.Services.Exhbition;
using Imagery.Service.Services.Image;
using Imagery.Service.Services.Topics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.API.Extensions
{
    public static class AppServiceExtensions
    {
        public static IServiceCollection AppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Imagery.API", Version = "v1" });
            });

            services.AddDbContext<ImageryContext>(options => options.UseSqlServer(configuration.GetConnectionString("DevBase")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExhibitionService, ExhibitionService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ITopicService, TopicService>();

            services.AddCors();

            return services;
        }
    }
}
