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
    public class RenewReserveToAsyncTest
    {
        [Theory, AutoData]
        public async Task RenewReserveToAsync_GivenExistentReserve_ThenRenewedReserveAddingMoreNumberOfDays(
            Guid reserveId,
            int days)
        {
            // Arrange
            var (sut, uow, reserveRepository, outboxMessagesReposity, _) = new VehiclesReserveServiceMockBuilder()
                .WithRenewReserveSuccess(reserveId, days)
                .BuildWithMocks();

            // Act
            await sut.RenewReserveToAsync(reserveId, days);

            // Arrange
            Mock.VerifyAll(uow, reserveRepository, outboxMessagesReposity);
        }

        [Theory, AutoData]
        public async Task RenewReserveToAsync_GivenNonexistentReserve_ThenNotificationAdded(
            Guid reserveId,
            int days)
        {
            // Arrange
            var (sut, uow, reserveRepository, _, notifications) = new VehiclesReserveServiceMockBuilder()
                .WithNonexistentReserve(reserveId)
                .BuildWithMocks();

            // Act
            await sut.RenewReserveToAsync(reserveId, days);

            // Arrange
            Mock.VerifyAll(uow, reserveRepository, notifications);
        }

        [Theory, AutoData]
        public async Task RenewReserveToAsync_GivenRenewFails_ThenThrowException(
            Guid reserveId,
            int days,
            Exception exception)
        {
            // Arrange
            var (sut, uow, reserveRepository, _, _) = new VehiclesReserveServiceMockBuilder()
                .WithRenewReserveFails(reserveId, days, exception)
                .BuildWithMocks();

            // Act
            Func<Task> func = async () =>
                await sut.RenewReserveToAsync(reserveId, days);

            // Assert
            await func.Should()
                .ThrowAsync<Exception>()
                .WithMessage(exception.Message);
            Mock.VerifyAll(uow, reserveRepository);
        }
    }
}
