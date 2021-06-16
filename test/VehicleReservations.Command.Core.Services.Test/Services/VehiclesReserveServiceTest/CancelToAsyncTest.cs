using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using VehicleReservations.Command.Core.Services.Services;
using VehicleReservations.Command.Core.Services.Test.Commons.Builders;
using Xunit;

namespace VehicleReservations.Command.Core.Services.Test.Services.VehiclesReserveServiceTest
{
    [Trait("Unit", nameof(VehiclesReserveService))]
    public class CancelToAsyncTest
    {
        [Theory, AutoData]
        public async Task CancelToAsync_GivenExistentReserve_ThenCancelThisReserve(Guid reserveId)
        {
            // Arrange
            var (sut, uow, reserveRepository, outboxMessagesReposity, _) = new VehiclesReserveServiceMockBuilder()
                .WithCancelReserveSuccess(reserveId)
                .BuildWithMocks();

            // Act
            await sut.CancelToAsync(reserveId);

            // Assert
            Mock.VerifyAll(uow, reserveRepository, outboxMessagesReposity);
        }

        [Theory, AutoData]
        public async Task CancelToAsync_GivenNonexistentReserve_ThenNotificationAdded(Guid reserveId)
        {
            // Arrange
            var (sut, uow, reserveRepository, _, notifications) = new VehiclesReserveServiceMockBuilder()
                .WithNonexistentReserve(reserveId)
                .BuildWithMocks();

            // Act
            await sut.CancelToAsync(reserveId);

            // Assert
            Mock.VerifyAll(uow, reserveRepository, notifications);
        }

        [Theory, AutoData]
        public async Task CancelToAsync_GivenUpdateFails_ThenThrowException(Guid reserveId, Exception exception)
        {
            // Arrange
            var (sut, uow, reserveRepository, _, _) = new VehiclesReserveServiceMockBuilder()
                .WithCancelReserveFailed(reserveId, exception)
                .BuildWithMocks();

            // Act
            Func<Task> func = async () =>
                await sut.CancelToAsync(reserveId);

            // Assert
            await func.Should()
                .ThrowAsync<Exception>()
                .WithMessage(exception.Message);
            Mock.VerifyAll(uow, reserveRepository);
        }
    }
}
