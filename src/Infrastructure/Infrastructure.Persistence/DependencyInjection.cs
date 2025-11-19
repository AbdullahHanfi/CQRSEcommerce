using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

using Data;
using Domain.Repositories;
using Implementation;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SQL"),
            b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

        // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}