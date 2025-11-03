
namespace Infrastructure.Shared;

using Application.Abstractions.Services;
using Configurations;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using StackExchange.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
                options.Configuration = configuration.GetConnectionString("Redis")
            );
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!)
        );
        services.Configure<DigitalOceanSpaceSettings>(configuration.GetSection("DigitalOceanSpace"));

        services.AddSingleton<IFileStorageService, FileStorageService>();
        services.AddSingleton<IImageService, ImageService>();

        // Scrutor has problems for register generic classes 
        // services.Decorate(typeof(IRepository<>), typeof(CachedRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(CachedRepository<>));


        return services;
    }
}
