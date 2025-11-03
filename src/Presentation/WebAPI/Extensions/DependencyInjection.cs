using WebAPI.Extensions.ServiceCollectionExtensions;

namespace WebAPI.Extensions;
public static class DependencyInjection
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSwagger(configuration);


        return services;
    }

}
