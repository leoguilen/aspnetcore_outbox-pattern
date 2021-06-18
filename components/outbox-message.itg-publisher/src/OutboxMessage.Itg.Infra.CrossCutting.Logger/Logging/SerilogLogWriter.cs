using System;
using OutboxMessage.Itg.Core.Configurations;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.CrossCutting.Logger.Models;
using Serilog;
using Serilog.Events;

namespace OutboxMessage.Itg.Infra.CrossCutting.Logger.Logging
{
    internal class SerilogLogWriter : ILogWriter
    {
        private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}";

        private readonly string _application;
        private readonly string _version;
        private readonly ILogger _logger;

        public SerilogLogWriter(AppSettings appSettings)
        {
            _application = appSettings.Application;
            _version = appSettings.Version;
            _logger = SetupLogger();
        }

        public void Error(string message, object data = null, Exception ex = null)
        {
            Log(message, data, ex, LogEventLevel.Error, _logger.Error);
        }

        public void Fatal(string message, object data = null, Exception ex = null)
        {
            Log(message, data, ex, LogEventLevel.Fatal, _logger.Fatal);
        }

        public void Info(string message, object data = null, Exception ex = null)
        {
            Log(message, data, ex, LogEventLevel.Information, _logger.Information);
        }

        public void Warn(string message, object data = null, Exception ex = null)
        {
            Log(message, data, ex, LogEventLevel.Warning, _logger.Warning);
        }

        private static ILogger SetupLogger() =>
            new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: DefaultOutputTemplate)
                .CreateLogger();

        private void Log(
            string message,
            object data,
            Exception ex,
            LogEventLevel level,
            Action<string, LogMessage> logger)
        {
            var logMessage = new LogMessage
            {
                Timestamp = DateTime.Now,
                Level = level.ToString(),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                Application = _application,
                Version = _version,
                Message = message,
                Data = data,
                Error = ex ?? null,
                ErrorMessage = ex?.Message,
            };

            logger.Invoke("{@LogMessage}", logMessage);
        }
    }
}
