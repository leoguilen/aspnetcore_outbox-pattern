using System;
using AutoFixture;
using FluentAssertions;
using VehicleReservations.Command.Core.Enums;
using VehicleReservations.Command.Core.Models;
using Xunit;

namespace VehicleReservations.Command.Core.Test.Models
{
    [Trait("Unit", nameof(VehicleReservation))]
    public class VehicleReservationTest
    {
        [Fact]
        public void Instance_GivenInitializeInstance_ThenCreateObject()
        {
            // Arrange
            var fixture = new Fixture();
            var expectedObject = new
            {
                ReserveId = Guid.NewGuid(),
                VehicleId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                ReservedAt = fixture.Create<DateTime>(),
                ReservationExpiresOn = fixture.Create<DateTime>(),
                Value = fixture.Create<decimal>(),
                Status = fixture.Create<ReserveStatus>(),
            };

            // Act
            var vehicleReservation = new VehicleReservation()
            {
                ReserveId = expectedObject.ReserveId,
                VehicleId = expectedObject.VehicleId,
                CustomerId = expectedObject.CustomerId,
                ReservedAt = expectedObject.ReservedAt,
                ReservationExpiresOn = expectedObject.ReservationExpiresOn,
                Value = expectedObject.Value,
                Status = expectedObject.Status,
            };

            // Assert
            vehicleReservation.Should().BeEquivalentTo(expectedObject);
        }
    }
}
