using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Moq;
using VehicleReservations.Command.ApplicationServices.Feature;
using VehicleReservations.Command.ApplicationServices.Features.CancelReserve;
using VehicleReservations.Command.Core.Interfaces.Services;
using Xunit;

namespace VehicleReservations.Command.ApplicationServices.Test.Handlers
{
    [Trait("Unit", nameof(CancelReserveCommandHandler))]
    public class CancelReserveCommandHandlerTest
    {
        private readonly Mock<IVehiclesReserveService> _vehiclesReserveService;

        public CancelReserveCommandHandlerTest()
        {
            _vehiclesReserveService = new(MockBehavior.Strict);
        }

        [Theory, AutoData]
        public async Task Handle_GivenSendCancelReserveCommand_ThenReserveShouldBeCancelled(CancelReserveCommand request)
        {
            // Arrange
            _vehiclesReserveService
                .Setup(x => x.CancelToAsync(request.ReserveId))
                .Returns(Task.CompletedTask);
            var sut = new CancelReserveCommandHandler(_vehiclesReserveService.Object);

            // Act
            await sut.Handle(request, CancellationToken.None);

            // Assert
            _vehiclesReserveService.VerifyAll();
        }

        [Theory, AutoData]
        public async Task Handle_GivenCancellationRequested_ThenThrowExpectedOperationCanceledException(
            CancellationTokenSource cancellationTokenSource,
            CancelReserveCommand request)
        {
            // Arrange
            cancellationTokenSource.Cancel();
            var canceledToken = cancellationTokenSource.Token;
            var sut = new CancelReserveCommandHandler(_vehiclesReserveService.Object);

            // Act
            Func<Task<Unit>> func = async () =>
                await sut.Handle(request, canceledToken);

            // Assert
            await func.Should()
                .ThrowExactlyAsync<OperationCanceledException>()
                .WithMessage("The operation was canceled.");
        }
    }
}
