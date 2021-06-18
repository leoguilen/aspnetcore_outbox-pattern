using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VehicleReservations.Command.Api.Controllers;
using VehicleReservations.Command.ApplicationServices.Feature;
using Xunit;

namespace VehicleReservations.Command.Api.Test.Controllers
{
    public class ReservationsControllerTest
    {
        private const string ControllerRoute = "/api/v1/reservations";
        private readonly Mock<IMediator> _mediator;

        public ReservationsControllerTest()
        {
            _mediator = new(MockBehavior.Strict);
        }

        [Theory, AutoData]
        public async Task CreateReserveAsync_GivenCreateReserveCommand_ThenCreateReserveAndReturnStatus201Created(CreateReserveCommand request)
        {
            // Arrange
            var expectedResult = new CreatedResult(string.Empty, request);
            _mediator
                .Setup(x => x.Send(request, CancellationToken.None))
                .ReturnsAsync(Unit.Value);
            var sut = GetController();

            // Act
            var result = await sut.CreateReserveAsync(request);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory, AutoData]
        public async Task CancelReserveAsync_GivenExistentReserveId_ThenCancelReserveAndReturnStatus200OK(Guid reserveId)
        {
            // Arrange
            var expectedResult = new OkResult();
            _mediator
                .Setup(x => x.Send(new CancelReserveCommand(reserveId), CancellationToken.None))
                .ReturnsAsync(Unit.Value);
            var sut = GetController();

            // Act
            var result = await sut.CancelReserveAsync(reserveId);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory, AutoData]
        public async Task RenewReserveAsync_GivenExistentReserveIdAndNumberOfDays_ThenRenewReserveAndReturnStatus200OK(Guid reserveId, int days)
        {
            // Arrange
            var expectedResult = new OkResult();
            _mediator
                .Setup(x => x.Send(new RenewReserveCommand(reserveId, days), CancellationToken.None))
                .ReturnsAsync(Unit.Value);
            var sut = GetController();

            // Act
            var result = await sut.RenewReserveAsync(reserveId, days);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        private ReservationsController GetController()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = ControllerRoute;
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };

            return new ReservationsController(_mediator.Object)
            {
                ControllerContext = controllerContext,
            };
        }
    }
}
