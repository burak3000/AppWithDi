using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

namespace AppWithDi
{
    public class HostedServiceImpl : BackgroundService
    {
        private readonly ILogger<HostedServiceImpl> _logger;
        private readonly IConfiguration _config;
        private readonly IHostApplicationLifetime _lifetime;

        public HostedServiceImpl(ILoggerFactory loggerFactory, IConfiguration config, IHostApplicationLifetime lifeTime)
        {
            _logger = loggerFactory.CreateLogger<HostedServiceImpl>();
            _config = config;
            _lifetime = lifeTime;
        }

        public async Task Run()
        {
            JobLogger jobLogger = new JobLogger("jobLogger.log");
            jobLogger.LogInformation("Hosted service impl is started.");
            int loopCount = _config.GetValue<int>("LoopCount");
            for (int i = 0; i < loopCount; i++)
            {
                await Task.Delay(1000);
                jobLogger.LogInformation($"{i}");
            }
            jobLogger.FlushLogsToFile();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Cancellation is not requested. Continueing job...");
                await Task.Delay(1000);
            }

            _logger.LogInformation("Cancellation is requested. Exiting from the app...");
            _lifetime.StopApplication();
        }
    }
}

