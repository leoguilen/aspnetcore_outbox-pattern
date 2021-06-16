using System;

namespace VehicleReservations.Command.Core.Events
{
    public record VehicleReserveCancelled(Guid ReserveId);
}
