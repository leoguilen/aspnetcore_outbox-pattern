using System;

namespace OutboxMessage.Itg.Infra.CrossCutting.Logger.Models
{
    internal record LogMessage
    {
        public DateTime Timestamp { get; init; }

        public Guid CorrelationId { get; set; }

        public string Level { get; init; }

        public string Environment { get; init; }

        public string Application { get; init; }

        public string Version { get; init; }

        public string Message { get; init; }

        public object Data { get; init; }

        public Exception Error { get; init; }

        public string ErrorMessage { get; init; }
    }
}
