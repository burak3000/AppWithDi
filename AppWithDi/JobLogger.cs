using Serilog;
using Serilog.Core;

namespace AppWithDi
{
    public class JobLogger : IDisposable
    {
        private readonly Logger _logger;
        private bool disposedValue;

        public JobLogger(string logFilePath)
        {
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _logger.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

