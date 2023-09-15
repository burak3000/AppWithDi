using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace AppWithDi
{
    public class JobLogger : IDisposable
    {
        private readonly string _logFilePath;
        private readonly Logger _logger;
        private bool disposedValue;

        public JobLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            _logger = new LoggerConfiguration().WriteTo.File(logFilePath ?? "Job.log").CreateLogger();
            _logger.Information("New job log is created successfully");
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogError(Exception ex, string message)
        {
            _logger.Error(ex, message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        public void FlushLogsToFile()
        {
            using (_logger)
            {
                _logger.Information("This job logger is terminating...");
            }
        }

        protected virtual async Task Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    await _logger.DisposeAsync();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~JobLogger()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

