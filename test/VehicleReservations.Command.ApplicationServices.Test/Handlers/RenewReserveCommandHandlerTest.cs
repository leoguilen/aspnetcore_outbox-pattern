using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Moq;
using VehicleReservations.Command.ApplicationServices.Feature;
using VehicleReservations.Command.ApplicationServices.Features.RenewReserve;
using VehicleReservations.Command.Core.Interfaces.Services;
using Xunit;

namespace VehicleReservations.Command.ApplicationServices.Test.Handlers
{
    [Trait("Unit", nameof(RenewReserveCommandHandler))]
    public class RenewReserveCommandHandlerTest
    {
        private readonly Mock<IVehiclesReserveService> _vehiclesReserveService;

        public RenewReserveCommandHandlerTest()
        {
            _vehiclesReserveService = new(MockBehavior.Strict);
        }

        [Theory, AutoData]
        public async Task Handle_GivenSendRenewReserveCommand_ThenReserveShouldBeRenewed(RenewReserveCommand request)
        {
            // Arrange
            _vehiclesReserveService
                .Setup(x => x.RenewReserveToAsync(request.ReserveId, request.Days))
                .Returns(Task.CompletedTask);
            var sut = new RenewReserveCommandHandler(_vehiclesReserveService.Object);

            // Act
            await sut.Handle(request, CancellationToken.None);

            // Assert
            _vehiclesReserveService.VerifyAll();
        }

        [Theory, AutoData]
        public async Task Handle_GivenCancellationRequested_ThenThrowExpectedOperationCanceledException(
            CancellationTokenSource cancellationTokenSource,
            RenewReserveCommand request)
        {
            // Arrange
            cancellationTokenSource.Cancel();
            var canceledToken = cancellationTokenSource.Token;
            var sut = new RenewReserveCommandHandler(_vehiclesReserveService.Object);

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
