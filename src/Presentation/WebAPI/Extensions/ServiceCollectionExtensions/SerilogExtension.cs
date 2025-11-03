using Serilog;

namespace WebAPI.Extensions.ServiceCollectionExtensions
{
    public static class SerilogExtension
    {
        public static IHostBuilder AddSerilog(this IHostBuilder host)
        {
            host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services));

            return host;
        }
    }
}
