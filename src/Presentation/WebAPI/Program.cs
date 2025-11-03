#pragma warning disable CS1591

using Infrastructure.Persistence;

namespace WebAPI;

using Application;
using Domain.Repositories;
using Infrastructure.Identity;
using Infrastructure.Persistence.Implementation;
using Infrastructure.Shared;
using Infrastructure.Shared.Services;
using Serilog;
using WebAPI.Extensions;
using WebAPI.Extensions.ServiceCollectionExtensions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        #region CORS Policy
        // --- Add CORS services and define a policy ---
        const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:4200") // my Angular dev port
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                              });
        });
        #endregion


        builder.Host.AddSerilog();

        //builder.Services.AddApplication();
        //builder.Services.AddInfrastructurePersistence(builder.Configuration);
        //builder.Services.AddInfrastructureIdentity(builder.Configuration);
        //builder.Services.AddSharedInfrastructure(builder.Configuration);
        //builder.Services.AddWebAPI(builder.Configuration);
        
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