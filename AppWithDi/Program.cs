using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AppWithDi // Note: actual namespace depends on the project name. 
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            IConfiguration config = builder.Build();

            string? logFileName = config.GetValue<string>("LogFileName");

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .WriteTo.Console()
                .WriteTo.File(logFileName ?? "app.log")
                .CreateLogger();

            logger.Information("Application is started");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<HostedServiceImpl>();
                    services.AddHostedService<HostedServiceImpl>();
                })
                .UseSerilog(logger)
                .Build();

            var service = ActivatorUtilities.CreateInstance<HostedServiceImpl>(host.Services);
            Task t1 = service.Run();
            Task t2 = host.RunAsync();

            await Task.WhenAny(t1, t2);
            logger.Information("App is finished. Exiting...");
        }

        static void BuildConfig(IConfigurationBuilder configurationBuilder)
        {
            string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            configurationBuilder.SetBasePath(exeDir)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
}