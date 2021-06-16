using System;
using MediatR;

namespace VehicleReservations.Command.ApplicationServices.Feature
{
    public record RenewReserveCommand : IRequest
    {
        public RenewReserveCommand(Guid reserveId, int days) =>
            (ReserveId, Days) = (reserveId, days);

        public Guid ReserveId { get; init; }

        public int Days { get; init; }
    }
}
