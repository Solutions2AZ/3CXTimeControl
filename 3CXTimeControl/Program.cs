using _3CXTimeControl.Models;
using _3CXTimeControl.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                services.AddDbContext<_3CXTimeControl.Models.DB.ControlLlamadasContext>(options => options.UseMySQL(connectionString));

                // Bind WebSocketSettings
                var settings = new Config3cxSettings();
                context.Configuration.GetSection("Config3CX").Bind(settings);

                // Register dependencies
                services.AddSingleton(settings);
                services.AddSingleton<MemoryStorageService>();
                services.AddSingleton<ConnectionService>();
                services.AddHostedService<WebSocketService>();
                services.AddSingleton<TaskSchedulerService>();
                services.AddHostedService(provider => provider.GetRequiredService<TaskSchedulerService>());
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();

await host.RunAsync();
