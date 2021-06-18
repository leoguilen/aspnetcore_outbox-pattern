using System;
using System.Threading.Tasks;
using VehicleReservations.Command.Core.Models;

namespace VehicleReservations.Command.Core.Interfaces.Services
{
    public interface IVehiclesReserveService
    {
        Task CreateFromAsync(VehicleReservation vehicleReservation);

        Task CancelToAsync(Guid reserveId);

        Task RenewReserveToAsync(Guid reserveId, int days);
    }
}
