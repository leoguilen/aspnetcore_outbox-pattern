using System;
using System.Threading.Tasks;
using Dapper;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Core.Models;
using VehicleReservations.Command.Infrastructure.Data.Repositories.Statements;

namespace VehicleReservations.Command.Infrastructure.Data.Repositories
{
    internal class ReserveRepository : IReserveRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReserveRepository(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task AddAsync(VehicleReservation vehicleReservation) =>
            await _unitOfWork.Connection.ExecuteAsync(
                sql: SqlStatements.InsertReserve,
                param: vehicleReservation,
                transaction: _unitOfWork.Transaction);

        public async Task<bool> CheckVehicleAvailableAsync(Guid vehicleId) =>
            await _unitOfWork.Connection.QueryFirstAsync<bool>(
                sql: SqlStatements.SelectVehicleAvailable,
                param: new { VehicleId = vehicleId });

        public async Task<bool> ExistsAsync(Guid reserveId) =>
            await _unitOfWork.Connection.QueryFirstAsync<bool>(
                sql: SqlStatements.SelectReserve,
                param: new { ReserveId = reserveId });

        public async Task UpdateExpireDateAsync(Guid reserveId, int days)
        {
            var newExpireDate = (await GetExpiredDateBy(reserveId)).AddDays(days);

            await _unitOfWork.Connection.ExecuteAsync(
                sql: SqlStatements.UpdateReserveExpireDate,
                param: new { ReservationExpiresOn = newExpireDate, ReserveId = reserveId },
                transaction: _unitOfWork.Transaction);
        }

        public async Task UpdateStatusAsync(Guid reserveId) =>
            await _unitOfWork.Connection.ExecuteAsync(
                sql: SqlStatements.UpdateReserveStatus,
                param: new { ReserveId = reserveId },
                transaction: _unitOfWork.Transaction);

        private async Task<DateTime> GetExpiredDateBy(Guid reserveId) =>
            await _unitOfWork.Connection.QueryFirstAsync<DateTime>(
                sql: SqlStatements.SelectReserveExpireDate,
                param: new { ReserveId = reserveId });
    }
}
