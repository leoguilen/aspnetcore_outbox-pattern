using System;
using System.Net;
using Serilog;
using Serilog.Events;
using VehicleReservations.Command.Core.Configurations;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Models;

namespace VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Logging
{
    internal class SerilogLogWriter : ILogWriter
    {
        private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}";

        private readonly string _application;
        private readonly string _version;
        private readonly ILogger _logger;

        private string _controller;
        private string _operation;
        private string _correlationId;
        private string _traceId;

        public SerilogLogWriter(AppSettings appSettings)
        {
            _application = appSettings.Application;
            _version = appSettings.Version;
            _logger = SetupLogger();
        }

        public void SetupLogger(
            string controller,
            string operation,
            Guid correlationId,
            string traceId)
        {
            _controller = controller;
            _operation = operation;
            _correlationId = correlationId.ToString();
            _traceId = traceId;
        }

        public void Error(string message, int? statusCode, object data = null, Exception ex = null)
        {
            Log(message, statusCode, data, ex, LogEventLevel.Error, _logger.Error);
        }

        public void Fatal(string message, int? statusCode, object data = null, Exception ex = null)
        {
            Log(message, statusCode, data, ex, LogEventLevel.Fatal, _logger.Fatal);
        }

        public void Info(string message, int? statusCode, object data = null, Exception ex = null)
        {
            Log(message, statusCode, data, ex, LogEventLevel.Information, _logger.Information);
        }

        public void Warn(string message, int? statusCode, object data = null, Exception ex = null)
        {
            Log(message, statusCode, data, ex, LogEventLevel.Warning, _logger.Warning);
        }

        private static ILogger SetupLogger() =>
            new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: DefaultOutputTemplate)
                .CreateLogger();

        private void Log(
            string message,
            int? statusCode,
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
                Controller = _controller,
                Action = _operation,
                Message = message,
                CorrelationId = _correlationId ?? Guid.NewGuid().ToString(),
                TraceId = _traceId,
                Data = data,
                Error = ex ?? null,
                ErrorMessage = ex?.Message,
            };

            if (statusCode is not null)
            {
                logMessage.IsSuccessful = statusCode.Value.ToString().StartsWith("2");
                logMessage.StatusCode = statusCode.Value;
                logMessage.StatusDescription = ((HttpStatusCode)statusCode.Value).ToString();
            }

            logger.Invoke("{@LogMessage}", logMessage);
        }
    }
}
