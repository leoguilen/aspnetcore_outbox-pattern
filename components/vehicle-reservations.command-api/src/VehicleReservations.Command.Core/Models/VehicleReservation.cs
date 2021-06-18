using System;
using VehicleReservations.Command.Core.Enums;

namespace VehicleReservations.Command.Core.Models
{
    public class VehicleReservation
    {
        public VehicleReservation() =>
            ReserveId = Guid.NewGuid();

        public Guid ReserveId { get; init; }

        public Guid VehicleId { get; init; }

        public Guid CustomerId { get; init; }

        public DateTime ReservedAt { get; init; }

        public DateTime ReservationExpiresOn { get; init; }

        public decimal Value { get; init; }

        public ReserveStatus Status { get; init; }
    }
}
