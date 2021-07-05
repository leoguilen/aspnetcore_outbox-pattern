using System;
using AutoFixture.Xunit2;
using Moq;
using OutboxMessage.Itg.Core.Configurations;
using OutboxMessage.Itg.Infra.CrossCutting.Logger.Logging;
using OutboxMessage.Itg.Infra.CrossCutting.Logger.Models;
using Serilog;
using Xunit;

namespace OutboxMessage.Itg.Infra.CrossCutting.Logger.Test.Logging
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
            object data,
            Exception exception)
        {
            // Arrange
            var sut = new SerilogLogWriter(appSettings);

            // Act
            sut.Error(message, data, exception);

            // Assert
            _logger.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<LogMessage>()));
        }
    }
}
