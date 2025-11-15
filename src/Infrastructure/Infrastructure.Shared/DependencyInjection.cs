namespace Infrastructure.Shared;

using Application.Abstractions.Services;
using Configurations;
using Domain.Repositories;
using Infrastructure.Persistence.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Services;
using StackExchange.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Redis Configuration
        var redisConnectionString = configuration.GetConnectionString("Redis") 
            ?? throw new InvalidOperationException("Redis connection string is not configured");
            
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "EcommerceApp_";
        });
        
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConnectionString)
        );

        // Digital Ocean Spaces Configuration
        services.Configure<DigitalOceanSpaceSettings>(configuration.GetSection("DigitalOceanSpace"));

        // Services
        services.AddSingleton<IFileStorageService, FileStorageService>();
        services.AddSingleton<IImageService, ImageService>();

        // Register the concrete repository first
        services.AddScoped(typeof(Repository<>));
        
        // Register CachedRepository with proper dependencies
        services.AddScoped(typeof(IRepository<>), typeof(CachedRepository<>));

        return services;
    }
}