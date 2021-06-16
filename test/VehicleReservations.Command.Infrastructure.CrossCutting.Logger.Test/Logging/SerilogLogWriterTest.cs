using System;
using System.Net;
using AutoFixture.Xunit2;
using Moq;
using Serilog;
using VehicleReservations.Command.Core.Configurations;
using VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Logging;
using VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Models;
using Xunit;

namespace Transaction.QueryApi.Infra.Logger.Test.Logging
{
    [Trait("Unit", nameof(SerilogLogWriter))]
    public class SerilogLogWriterTest
    {
        private readonly Mock<ILogger> _logger;

        public SerilogLogWriterTest()
        {
            _logger = new(MockBehavior.Strict);
        }

        [Theory(Skip = "Skip"), AutoData]
        public void Error_WhenCall_ThenWriteMessage(
            AppSettings appSettings,
            string message,
            HttpStatusCode statusCode,
            object data,
            Exception exception)
        {
            // Arrange
            var sut = new SerilogLogWriter(appSettings);

            // Act
            sut.Error(message, (int)statusCode, data, exception);

            // Assert
            _logger.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<LogMessage>()));
        }
    }
}
