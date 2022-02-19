using API.Data;
using API.Hoppers;
using API.Interfaces;
using API.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config )
        {   

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            // services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<DataContext>(options =>
            // services.AddDbContext<DataContext>(options =>
            {
                
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            }
            );

            return services;
        }
    }
}