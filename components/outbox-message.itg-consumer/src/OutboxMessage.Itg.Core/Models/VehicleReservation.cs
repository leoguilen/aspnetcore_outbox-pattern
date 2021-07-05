using System;
using System.Text.Json.Serialization;
using OutboxMessage.Itg.Core.Enums;

namespace OutboxMessage.Itg.Core.Models
{
    public class VehicleReservation
    {
        [JsonPropertyName("reserveId")]
        public Guid ReserveId { get; init; }

        [JsonPropertyName("vehicleId")]
        public Guid VehicleId { get; init; }

        [JsonPropertyName("customerId")]
        public Guid CustomerId { get; init; }

        [JsonPropertyName("reservedAt")]
        public DateTime ReservedAt { get; init; }

        [JsonPropertyName("reservationExpiresOn")]
        public DateTime ReservationExpiresOn { get; init; }

        [JsonPropertyName("value")]
        public decimal Value { get; init; }

        [JsonPropertyName("status")]
        public ReserveStatus Status { get; init; }
    }
}
