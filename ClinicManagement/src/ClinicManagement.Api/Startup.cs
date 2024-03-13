using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using BlazorShared;
using ClinicManagement.Api.Hubs;
using ClinicManagement.Core.Interfaces;
using ClinicManagement.Infrastructure;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Messaging;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace ClinicManagement.Api
{
  public class Startup
  {
    // Constant for CORS policy
    public const string CORS_POLICY = "CorsPolicy";

    // Environment variable to determine the current hosting environment
    private readonly IWebHostEnvironment _env;

    // Constructor for Startup
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      _env = env;
    }

    // Configuration property to access the configuration settings
    public IConfiguration Configuration { get; }

    // Method to configure services during development
    public void ConfigureDevelopmentServices(IServiceCollection services)
    {
      // use in-memory database
      //ConfigureInMemoryDatabases(services);

      // use real database
      ConfigureProductionServices(services); //original
    }

    // Method to configure services for Docker environment
    public void ConfigureDockerServices(IServiceCollection services)
    {
      ConfigureDevelopmentServices(services);
    }

    private void ConfigureInMemoryDatabases(IServiceCollection services)
    {
      services.AddDbContext<AppDbContext>(c =>
          c.UseInMemoryDatabase("AppDb"));

      ConfigureServices(services);
    }

    // Method to configure in-memory databases for testing
    public void ConfigureTestingServices(IServiceCollection services)
    {
      ConfigureInMemoryDatabases(services);
    }

    // Method to configure services during production
    public void ConfigureProductionServices(IServiceCollection services)
    {
      // use real database
      // Requires LocalDB which can be installed with SQL Server Express 2016
      // https://www.microsoft.com/en-us/download/details.aspx?id=54284
      services.AddDbContext<AppDbContext>(c =>
          c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      ConfigureServices(services);
    }

    // Method to configure the Autofac container
    public void ConfigureContainer(ContainerBuilder builder)
    {
      bool isDevelopment = (_env.EnvironmentName == "Development");
      builder.RegisterModule(new DefaultInfrastructureModule(isDevelopment, Assembly.GetExecutingAssembly()));
    }


    // Method to configure common services shared by different environments
    public void ConfigureServices(IServiceCollection services)
    {
      // Add SignalR for real-time communication
      services.AddSignalR();

      // Add memory cache
      services.AddMemoryCache();

      // Singleton registration for IApplicationSettings
      services.AddSingleton(typeof(IApplicationSettings), typeof(OfficeSettings));

      // Read base URL configuration from app settings
      var baseUrlConfig = new BaseUrlConfiguration();
      Configuration.Bind(BaseUrlConfiguration.CONFIG_NAME, baseUrlConfig);

      // CORS policy configuration
      services.AddCors(options =>
      {
        options.AddPolicy(name: CORS_POLICY,
                          builder =>
                          {
                            builder.WithOrigins(baseUrlConfig.WebBase.Replace("host.docker.internal", "localhost").TrimEnd('/'), "localhost:6100", "localhost:6150");
                            builder.SetIsOriginAllowed(origin => true);
                            builder.AllowAnyMethod();
                            builder.AllowAnyHeader();
                          });
      });

      // Add controllers, MediatR, response compression, AutoMapper, and Swagger
      services.AddControllers();
      services.AddMediatR(typeof(Startup).Assembly);
      services.AddResponseCompression(opts =>
      {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] { "application/octet-stream" });
      });
      services.AddAutoMapper(typeof(Startup).Assembly);
      services.AddSwaggerGenCustom();

      // Configure messaging using RabbitMQ
      var messagingConfig = Configuration.GetSection("RabbitMq");
      services.Configure<RabbitMqConfiguration>(messagingConfig);
      services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
      services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();

      //Add file service
      services.AddScoped(typeof(FileService));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // Use response compression middleware
      app.UseResponseCompression();

      // Enable developer exception page in development environment
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // Use HTTPS redirection (uncomment if needed)
      app.UseHttpsRedirection();

      app.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "images", "patients")),
        RequestPath = "/images/patients"
      });

      app.UseRouting();

      // Enable CORS using the configured policy
      app.UseCors(CORS_POLICY);

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI to app root
      });

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        // Uncomment if using SignalR
        //endpoints.MapHub<ClinicManagementHub>($"/{SignalRConstants.HUB_NAME}");
      });
    }
  }
}
