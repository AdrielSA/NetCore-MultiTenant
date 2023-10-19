using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Application.Options;
using MultiTenant.Application.Services;
using MultiTenant.Domain.Contracts.IRepositories;
using MultiTenant.Domain.Contracts.IServices;
using MultiTenant.Infrastructure.DbContexts;
using MultiTenant.Infrastructure.Repositories;
using System.Text;

namespace MultiTenant.Infrastructure.Extensions
{
    public static class InversionOfControlExtension
    {
        public static IServiceCollection AddDbConfigurations(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<MasterDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("MasterConnection")).EnableSensitiveDataLogging());
            services.AddDbContext<ProductsDbContext>();

            return services;
        }

        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            // UnitOfWorks
            services.AddTransient<IMasterUnitOfWork, MasterUnitOfWork>();
            services.AddTransient<IProductUnitOfWork, ProductUnitOfWork>();

            // Options
            services.Configure<AuthOptions>(configuration.GetSection(nameof(AuthOptions)));

            // Services
            services.AddScoped<IProviderService, ProviderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["AuthOptions:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["AuthOptions:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthOptions:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
