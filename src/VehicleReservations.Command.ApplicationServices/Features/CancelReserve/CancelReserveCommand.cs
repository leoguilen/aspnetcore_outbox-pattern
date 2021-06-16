using System;
using MediatR;

namespace VehicleReservations.Command.ApplicationServices.Feature
{
    public record CancelReserveCommand : IRequest
    {
        public CancelReserveCommand(Guid reserveId) => ReserveId = reserveId;

        public Guid ReserveId { get; init; }
    }
}
