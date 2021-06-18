using System;
using System.Threading.Tasks;
using Moq;
using VehicleReservations.Command.Core.Holders;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Core.Models;
using VehicleReservations.Command.Core.Notifications;
using VehicleReservations.Command.Core.Services.Services;

namespace VehicleReservations.Command.Core.Services.Test.Commons.Builders
{
    internal class VehiclesReserveServiceMockBuilder
    {
        private readonly Mock<IReserveRepository> _reserveRepository;
        private readonly Mock<IOutboxMessagesRepository> _outboxMessagesRepository;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<INotification> _notifications;
        private readonly Mock<IRequestContextHolder> _requestContextHolder;

        public VehiclesReserveServiceMockBuilder()
        {
            _reserveRepository = new(MockBehavior.Strict);
            _outboxMessagesRepository = new(MockBehavior.Strict);
            _uow = new(MockBehavior.Strict);
            _notifications = new(MockBehavior.Strict);
            _requestContextHolder = new(MockBehavior.Strict);

            _requestContextHolder.SetupGet(x => x.ApplicationName).Returns("vehicle-reservations.command-api");
            _requestContextHolder.SetupGet(x => x.CorrelationId).Returns(Guid.NewGuid());
        }

        public VehiclesReserveServiceMockBuilder WithNonexistentReserve(Guid reserveId)
        {
            _reserveRepository
                .Setup(x => x.ExistsAsync(reserveId))
                .ReturnsAsync(false);
            _notifications
                .Setup(x => x.Add("Reserve does not exists", 404));

            return this;
        }

        public VehiclesReserveServiceMockBuilder WithRegisterReserveAndVehicleNotAvailable(VehicleReservation vehicleReservation)
        {
            const string notificationMsg = "Vehicle not available for this reserve";
            const int notificationStsCode = 422;

            _reserveRepository
                .Setup(x => x.CheckVehicleAvailableAsync(vehicleReservation.VehicleId))
                .ReturnsAsync(false);
            _notifications
                .Setup(x => x.Add(notificationMsg, notificationStsCode));

            return this;
        }

        public VehiclesReserveServiceMockBuilder WithCancelReserveFailed(Guid reserveId, Exception exception)
        {
            _reserveRepository
                .Setup(x => x.ExistsAsync(reserveId))
                .ReturnsAsync(true);
            _uow.Setup(x => x.BeginTransaction());
            _reserveRepository
                .Setup(x => x.UpdateStatusAsync(reserveId))
                .Throws(exception);
            _uow.Setup(x => x.Rollback());
            _notifications
                .Setup(x => x.Add(exception, 500));

            return this;
        }

        public VehiclesReserveServiceMockBuilder WithRegisterReserveFailed(VehicleReservation vehicleReservation, Exception exception)
        {
            _reserveRepository
                .Setup(x => x.CheckVehicleAvailableAsync(vehicleReservation.VehicleId))
                .ReturnsAsync(true);
            _uow.Setup(x => x.BeginTransaction());
            _reserveRepository
                .Setup(x => x.AddAsync(vehicleReservation))
                .Throws(exception);
            _uow.Setup(x => x.Rollback());
            _notifications
                .Setup(x => x.Add(exception, 500));

            return this;
        }

        public VehiclesReserveServiceMockBuilder WithRenewReserveFails(Guid reserveId, int days, Exception exception)
        {
            _reserveRepository
                .Setup(x => x.ExistsAsync(reserveId))
                .ReturnsAsync(true);
            _uow.Setup(x => x.BeginTransaction());
            _reserveRepository
                .Setup(x => x.UpdateExpireDateAsync(reserveId, days))
                .Throws(exception);
            _uow.Setup(x => x.Rollback());
            _notifications
                .Setup(x => x.Add(exception, 500));

            return this;
        }

        public VehiclesReserveServiceMockBuilder WithCancelReserveSuccess(Guid reserveId)
        {
            SetupMockWithExistentReserve(reserveId);
            _reserveRepository
                .Setup(x => x.UpdateStatusAsync(reserveId))
                .Returns(Task.CompletedTask);

            return this;
        }

        public VehiclesReserveServiceMockBuilder WithRenewReserveSuccess(Guid reserveId, int days)
        {
            SetupMockWithExistentReserve(reserveId);
            _reserveRepository
                .Setup(x => x.UpdateExpireDateAsync(reserveId, days))
                .Returns(Task.CompletedTask);

            return this;
        }

        public VehiclesReserveServiceMockBuilder WithRegisterReserveSuccess(VehicleReservation vehicleReservation)
        {
            _reserveRepository
                .Setup(x => x.CheckVehicleAvailableAsync(vehicleReservation.VehicleId))
                .ReturnsAsync(true);
            _uow.Setup(x => x.BeginTransaction());
            _reserveRepository
                .Setup(x => x.AddAsync(vehicleReservation))
                .Returns(Task.CompletedTask);
            _outboxMessagesRepository
                .Setup(x => x.AddAsync(It.IsAny<OutboxMessage>()))
                .Returns(Task.CompletedTask);
            _uow.Setup(x => x.Commit());

            return this;
        }

        public VehiclesReserveService Build() => new(
            _uow.Object,
            _reserveRepository.Object,
            _outboxMessagesRepository.Object,
            _notifications.Object,
            _requestContextHolder.Object);

        public (VehiclesReserveService, Mock<IUnitOfWork>, Mock<IReserveRepository>, Mock<IOutboxMessagesRepository>, Mock<INotification>) BuildWithMocks() => (
            new(
                _uow.Object,
                _reserveRepository.Object,
                _outboxMessagesRepository.Object,
                _notifications.Object,
                _requestContextHolder.Object),
            _uow,
            _reserveRepository,
            _outboxMessagesRepository,
            _notifications);

        private void SetupMockWithExistentReserve(Guid reserveId)
        {
            _reserveRepository
                .Setup(x => x.ExistsAsync(reserveId))
                .ReturnsAsync(true);
            _uow.Setup(x => x.BeginTransaction());
            _outboxMessagesRepository
                .Setup(x => x.AddAsync(It.IsAny<OutboxMessage>()))
                .Returns(Task.CompletedTask);
            _uow.Setup(x => x.Commit());
        }
    }
}
