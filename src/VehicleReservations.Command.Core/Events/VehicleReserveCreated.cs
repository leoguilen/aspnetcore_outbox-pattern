using System;
using VehicleReservations.Command.Core.Enums;
using VehicleReservations.Command.Core.Models;

namespace VehicleReservations.Command.Core.Events
{
    public record VehicleReserveCreated
    {
        public Guid ReserveId { get; init; }

        public Guid VehicleId { get; init; }

        public Guid CustomerId { get; init; }

        public DateTime ReservedAt { get; init; }

        public DateTime ReservationExpiresOn { get; init; }

        public decimal Value { get; init; }

        public ReserveStatus Status { get; init; }

        public static VehicleReserveCreated From(VehicleReservation vehicleReservation) => new()
        {
            ReserveId = vehicleReservation.ReserveId,
            VehicleId = vehicleReservation.VehicleId,
            CustomerId = vehicleReservation.CustomerId,
            ReservedAt = vehicleReservation.ReservedAt,
            ReservationExpiresOn = vehicleReservation.ReservationExpiresOn,
            Value = vehicleReservation.Value,
            Status = vehicleReservation.Status,
        };
    };
}
