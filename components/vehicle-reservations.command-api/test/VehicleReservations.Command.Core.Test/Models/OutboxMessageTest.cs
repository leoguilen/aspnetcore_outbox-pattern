using System;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using VehicleReservations.Command.Core.Enums;
using VehicleReservations.Command.Core.Models;
using Xunit;

namespace VehicleReservations.Command.Core.Test.Models
{
    [Trait("Unit", nameof(OutboxMessage))]
    public class OutboxMessageTest
    {
        [Theory, AutoData]
        public void Instance_GivenConstructorWithPayload_ThenInitializeProperties(Fixture fixture)
        {
            // Arrange
            var payload = fixture.Create<object>();
            var applicationName = fixture.Create<string>();
            var correlationId = fixture.Create<Guid>();

            // Act
            var outboxMessage = new OutboxMessage(payload, applicationName, correlationId);

            // Assert
            outboxMessage.Event.Should().Be(payload.GetType().Name);
            outboxMessage.State.Should().Be(OutboxMessageState.ReadyToSend);
            outboxMessage.EmitedOn.Should().BeOnOrBefore(DateTime.Now);
        }

        [Theory, AutoData]
        public void Instance_GivenConstructorWithNullPayload_ThenThrowNullReferenceException(Fixture fixture)
        {
            // Arrange
            object payload = null;
            var applicationName = fixture.Create<string>();
            var correlationId = fixture.Create<Guid>();

            // Act
            Action act = () => _ = new OutboxMessage(payload, applicationName, correlationId);

            // Assert
            act.Should().Throw<NullReferenceException>();
        }
    }
}
