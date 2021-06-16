using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Moq;
using VehicleReservations.Command.ApplicationServices.Feature;
using VehicleReservations.Command.ApplicationServices.Features.CreateReserve;
using VehicleReservations.Command.Core.Interfaces.Services;
using VehicleReservations.Command.Core.Models;
using Xunit;

namespace VehicleReservations.Command.ApplicationServices.Test.Handlers
{
    [Trait("Unit", nameof(CreateReserveCommandHandler))]
    public class CreateReserveCommandHandlerTest
    {
        private readonly Mock<IVehiclesReserveService> _vehiclesReserveService;

        public CreateReserveCommandHandlerTest()
        {
            _vehiclesReserveService = new(MockBehavior.Strict);
        }

        [Theory, AutoData]
        public async Task Handle_GivenSendCreateReserveCommand_ThenNewReserveShouldBeCreated(CreateReserveCommand request)
        {
            // Arrange
            _vehiclesReserveService
                .Setup(x => x.CreateFromAsync(It.IsAny<VehicleReservation>()))
                .Returns(Task.CompletedTask);
            var sut = new CreateReserveCommandHandler(_vehiclesReserveService.Object);

            // Act
            await sut.Handle(request, CancellationToken.None);

            // Assert
            _vehiclesReserveService.VerifyAll();
        }

        [Theory, AutoData]
        public async Task Handle_GivenCancellationRequested_ThenThrowExpectedOperationCanceledException(
            CancellationTokenSource cancellationTokenSource,
            CreateReserveCommand request)
        {
            // Arrange
            cancellationTokenSource.Cancel();
            var canceledToken = cancellationTokenSource.Token;
            var sut = new CreateReserveCommandHandler(_vehiclesReserveService.Object);

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
