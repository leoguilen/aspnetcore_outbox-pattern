using System;

namespace VehicleReservations.Command.Core.Events
{
    public record VehicleReserveRenewed(Guid ReserveId, int Days);
}
