#pragma warning disable CS1591

using Application;
using Infrastructure.Persistence;
using Infrastructure.Identity;
using Infrastructure.Shared;
using Serilog;
using WebAPI.Extensions;
using WebAPI.Extensions.ServiceCollectionExtensions;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add environment variables with fallback
        builder.Configuration.AddEnvironmentVariables("ECOMMERCE_");

        #region CORS Policy
        const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                              });
        });
        #endregion

        builder.Host.AddSerilog();
        
        builder.Services
            .AddApplication()
            .AddInfrastructurePersistence(builder.Configuration)
            .AddInfrastructureIdentity(builder.Configuration)
            .AddSharedInfrastructure(builder.Configuration)
            .AddWebAPI(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }
        else
        {
            app.UseCors(myAllowSpecificOrigins);
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
#pragma warning restore CS1591