using System;
using System.Text.Json;
using VehicleReservations.Command.Core.Enums;

namespace VehicleReservations.Command.Core.Models
{
    public class OutboxMessage
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public OutboxMessage(object payload, string applicationName, Guid correlationId)
        {
            Application = applicationName;
            Event = payload.GetType().Name;
            CorrelationId = correlationId;
            Payload = JsonSerializer.Serialize(payload, _jsonSerializerOptions);
            State = OutboxMessageState.ReadyToSend;
            EmitedOn = DateTime.Now;
        }

        public string Application { get; init; }

        public string Event { get; init; }

        public Guid CorrelationId { get; init; }

        public string Payload { get; init; }

        public OutboxMessageState State { get; init; }

        public DateTime EmitedOn { get; init; }

        public DateTime? ModifiedOn { get; init; }
    }
}
