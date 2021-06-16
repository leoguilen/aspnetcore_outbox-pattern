using System;
using System.Threading.Tasks;
using VehicleReservations.Command.Core.Models;

namespace VehicleReservations.Command.Core.Interfaces.Infrastructure
{
    public interface IReserveRepository
    {
        Task AddAsync(VehicleReservation vehicleReservation);

        Task<bool> ExistsAsync(Guid reserveId);

        Task<bool> CheckVehicleAvailableAsync(Guid vehicleId);

        Task UpdateExpireDateAsync(Guid reserveId, int days);

        Task UpdateStatusAsync(Guid reserveId);
    }
}
