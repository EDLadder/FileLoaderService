using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using service_app;
using service_app.Configuration;
using service_app.Data;
using service_app.Services;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // load configuration
        IConfiguration configuration = hostContext.Configuration;

        // set connection string
        AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection");

        // set db configuration
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseNpgsql(AppSettings.ConnectionString);

        services.AddScoped<DataContext>(db => new DataContext(optionsBuilder.Options));
        services.Configure<IConfiguration>(configuration);
        // add sft service
        services.AddTransient<ISftpService, SftpService>();
        services.AddHostedService<Worker>();
    })
    .Build();

CreateDbIfNoneExist(host);

await host.RunAsync();

// check if database is created and table defined
static void CreateDbIfNoneExist(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var service = scope.ServiceProvider;

        try
        {
            var context = service.GetRequiredService<DataContext>();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            Console.Write(ex);
            throw;
        }
    }
}
