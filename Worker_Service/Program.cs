using Worker_Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Worker_Service.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Worker_Service.Services;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();

        CreateDbIfNoneExist(host);

        host.Run();
    }

    private static void CreateDbIfNoneExist(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var service = scope.ServiceProvider;
            var logger = service.GetRequiredService<ILogger>();
            try
            {
                logger.Information("Create database if it is not created.");
                var context = service.GetRequiredService<EmployeeDBContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                logger.Error($"Error occurred in database creation: {ex.Message}");
                throw ex;
            }
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<EmployeeDBContext>(options =>
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("DatabaseConnection"))
                    );
                services.AddScoped<IDataRepository, EmployeeDBHelper>();
                services.AddHostedService<Worker>();
                services.AddLogging(configure => configure.AddSerilog());
            })
            .UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(hostingContext.Configuration));
}
