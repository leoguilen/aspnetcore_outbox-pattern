using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using VehicleReservations.Command.Core.Models;
using VehicleReservations.Command.Core.Services.Services;
using VehicleReservations.Command.Core.Services.Test.Commons.Builders;
using Xunit;

namespace VehicleReservations.Command.Core.Services.Test.Services.VehiclesReserveServiceTest
{
    [Trait("Unit", nameof(VehiclesReserveService))]
    public class CreateFromAsyncTest
    {
        [Theory, AutoData]
        public async Task CreateFromAsync_GivenNewVehicleReserve_ThenRegisterReserve(VehicleReservation vehicleReservation)
        {
            // Arrange
            var (sut, uow, reserveRepository, outboxMessagesReposity, _) = new VehiclesReserveServiceMockBuilder()
                .WithRegisterReserveSuccess(vehicleReservation)
                .BuildWithMocks();

            // Act
            await sut.CreateFromAsync(vehicleReservation);

            // Assert
            Mock.VerifyAll(uow, reserveRepository, outboxMessagesReposity);
        }

        [Theory, AutoData]
        public async Task CreateFromAsync_GivenVehicleReserveWithVehicleNotAvailable_ThenNotificationAdded(VehicleReservation vehicleReservation)
        {
            // Arrange
            var (sut, uow, reserveRepository, _, notifications) = new VehiclesReserveServiceMockBuilder()
                .WithRegisterReserveAndVehicleNotAvailable(vehicleReservation)
                .BuildWithMocks();

            // Act
            await sut.CreateFromAsync(vehicleReservation);

            // Assert
            Mock.VerifyAll(uow, reserveRepository, notifications);
        }

        [Theory, AutoData]
        public async Task CreateFromAsync_GivenAddFails_ThenNotificationWithExceptionAdded(
            VehicleReservation vehicleReservation,
            Exception exception)
        {
            // Arrange
            var (sut, uow, reserveRepository, _, _) = new VehiclesReserveServiceMockBuilder()
                .WithRegisterReserveFailed(vehicleReservation, exception)
                .BuildWithMocks();

            // Act
            Func<Task> func = async () =>
                await sut.CreateFromAsync(vehicleReservation);

            // Assert
            await func.Should()
                .ThrowAsync<Exception>()
                .WithMessage(exception.Message);
            Mock.VerifyAll(uow, reserveRepository);
        }
    }
}
