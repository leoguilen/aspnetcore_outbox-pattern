using System;

namespace VehicleReservations.Command.Core.Interfaces.Infrastructure
{
    public interface ILogWriter
    {
        void SetupLogger(
            string controller,
            string operation,
            Guid correlationId,
            string traceId);

        void Info(string message, int? statusCode, object data = default, Exception ex = default);

        void Warn(string message, int? statusCode, object data = default, Exception ex = default);

        void Error(string message, int? statusCode, object data = default, Exception ex = default);

        void Fatal(string message, int? statusCode, object data = default, Exception ex = default);
    }
}
