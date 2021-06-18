using System;
using System.Net;
using System.Threading.Tasks;
using VehicleReservations.Command.Core.Events;
using VehicleReservations.Command.Core.Holders;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Core.Interfaces.Services;
using VehicleReservations.Command.Core.Models;
using VehicleReservations.Command.Core.Notifications;

namespace VehicleReservations.Command.Core.Services.Services
{
    internal class VehiclesReserveService : IVehiclesReserveService
    {
        private readonly IUnitOfWork _uow;
        private readonly IReserveRepository _reserveRepository;
        private readonly IOutboxMessagesRepository _outboxMessagesRepository;
        private readonly INotification _notification;
        private readonly IRequestContextHolder _requestContextHolder;

        public VehiclesReserveService(
            IUnitOfWork uow,
            IReserveRepository reserveRepository,
            IOutboxMessagesRepository outboxMessagesRepository,
            INotification notification,
            IRequestContextHolder requestContextHolder)
        {
            _uow = uow;
            _reserveRepository = reserveRepository;
            _outboxMessagesRepository = outboxMessagesRepository;
            _notification = notification;
            _requestContextHolder = requestContextHolder;
        }

        public async Task CancelToAsync(Guid reserveId)
        {
            var existsReserve = await _reserveRepository
                .ExistsAsync(reserveId);

            if (!existsReserve)
            {
                _notification.Add(
                    "Reserve does not exists",
                    (int)HttpStatusCode.NotFound);
                return;
            }

            _uow.BeginTransaction();
            try
            {
                await _reserveRepository
                    .UpdateStatusAsync(reserveId);
                await _outboxMessagesRepository
                    .AddAsync(new OutboxMessage(
                        new VehicleReserveCancelled(reserveId),
                        _requestContextHolder.ApplicationName,
                        _requestContextHolder.CorrelationId));

                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
        }

        public async Task CreateFromAsync(VehicleReservation vehicleReservation)
        {
            var isAvalibleVehicle = await _reserveRepository
                .CheckVehicleAvailableAsync(vehicleReservation.VehicleId);

            if (!isAvalibleVehicle)
            {
                _notification.Add(
                    "Vehicle not available for this reserve",
                    (int)HttpStatusCode.UnprocessableEntity);
                return;
            }

            _uow.BeginTransaction();
            try
            {
                await _reserveRepository
                    .AddAsync(vehicleReservation);
                await _outboxMessagesRepository
                    .AddAsync(new OutboxMessage(
                        VehicleReserveCreated.From(vehicleReservation),
                        _requestContextHolder.ApplicationName,
                        _requestContextHolder.CorrelationId));

                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
        }

        public async Task RenewReserveToAsync(Guid reserveId, int days)
        {
            var existsReserve = await _reserveRepository
                .ExistsAsync(reserveId);

            if (!existsReserve)
            {
                _notification.Add(
                    "Reserve does not exists",
                    (int)HttpStatusCode.NotFound);
                return;
            }

            _uow.BeginTransaction();
            try
            {
                await _reserveRepository
                    .UpdateExpireDateAsync(reserveId, days);
                await _outboxMessagesRepository
                    .AddAsync(new OutboxMessage(
                        new VehicleReserveRenewed(reserveId, days),
                        _requestContextHolder.ApplicationName,
                        _requestContextHolder.CorrelationId));

                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
        }
    }
}
