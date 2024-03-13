using System;
using Autofac.Extensions.DependencyInjection;
using ClinicManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Api
{
  public class Program
  {
    public static async System.Threading.Tasks.Task Main(string[] args)
    {
      // Create the host using the configuration defined in CreateHostBuilder
      var host = CreateHostBuilder(args)
                  .Build();

      // Use a scope to manage the services
      using (var scope = host.Services.CreateScope())
      {
        // Retrieve necessary services
        var services = scope.ServiceProvider;
        var hostEnvironment = services.GetService<IWebHostEnvironment>();
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<Program>();

        // Log information about the environment the application is starting in
        logger.LogInformation($"Starting in environment {hostEnvironment.EnvironmentName}");

        try
        {
          // Attempt to seed the database using the AppDbContextSeed service
          var seedService = services.GetRequiredService<AppDbContextSeed>();
          await seedService.SeedAsync(new OfficeSettings().TestDate);
        }
        catch (Exception ex)
        {
          // Log an error if an exception occurs during database seeding
          logger.LogError(ex, "An error occurred seeding the DB.");
        }
      }

      // Run the host, starting the application
      host.Run();
    }

    // Define the host configuration using CreateDefaultBuilder and configure it for web hosting
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            // Use Autofac as the IoC container
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
              // Specify the Startup class for web configuration
              webBuilder.UseStartup<Startup>();
            });


  }

}
