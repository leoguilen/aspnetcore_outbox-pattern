using System;

namespace VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Models
{
    internal record LogMessage
    {
        public DateTime Timestamp { get; init; }

        public string Level { get; init; }

        public string Environment { get; init; }

        public string Application { get; init; }

        public string Version { get; init; }

        public string Controller { get; init; }

        public string Action { get; init; }

        public bool? IsSuccessful { get; set; }

        public int? StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public string Message { get; init; }

        public string CorrelationId { get; init; }

        public string TraceId { get; init; }

        public object Data { get; init; }

        public Exception Error { get; init; }

        public string ErrorMessage { get; init; }
    }
}
