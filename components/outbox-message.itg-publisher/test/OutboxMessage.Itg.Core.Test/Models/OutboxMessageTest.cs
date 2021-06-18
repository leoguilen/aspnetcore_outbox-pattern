using System;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using OutboxMessage.Itg.Core.Enums;
using Xunit;

namespace OutboxMessage.Itg.Core.Test.Models
{
    [Trait("Unit", "OutboxMessage")]
    public class OutboxMessageTest
    {
        [Theory, AutoData]
        public void Instance_GivenValidProperties_ThenCreateNewInstance(Fixture fixture)
        {
            // Arrange
            var expectedInstance = new
            {
                Id = Guid.NewGuid(),
                Application = fixture.Create<string>(),
                Event = fixture.Create<string>(),
                CorrelationId = Guid.NewGuid(),
                Payload = fixture.Create<string>(),
                State = fixture.Create<OutboxMessageState>(),
                EmitedOn = fixture.Create<DateTime>(),
            };

            // Act
            var result = new Core.Models.OutboxMessage()
            {
                Id = expectedInstance.Id,
                Application = expectedInstance.Application,
                Event = expectedInstance.Event,
                CorrelationId = expectedInstance.CorrelationId,
                Payload = expectedInstance.Payload,
                State = expectedInstance.State,
                EmitedOn = expectedInstance.EmitedOn,
            };

            // Assert
            result.Should().BeEquivalentTo(expectedInstance);
        }
    }
}
