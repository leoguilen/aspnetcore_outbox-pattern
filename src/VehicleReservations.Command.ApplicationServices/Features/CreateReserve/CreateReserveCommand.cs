using System;
using MediatR;

namespace VehicleReservations.Command.ApplicationServices.Feature
{
    public record CreateReserveCommand : IRequest
    {
        public Guid VehicleId { get; init; }

        public Guid CustomerId { get; init; }

        public DateTime ReservedAt { get; init; }

        public DateTime ReservationExpiresOn { get; init; }

        public decimal Value { get; init; }

        public int Status { get; init; }
    }
}
