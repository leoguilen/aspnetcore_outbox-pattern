using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Moq;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Core.Services.Services;
using OutboxMessage.Itg.Core.Services.Test.Commons.Builders;
using Xunit;

namespace OutboxMessage.Itg.Core.Services.Test.Services
{
    [Trait("Unit", nameof(OutboxMessageItgService))]
    public class OutboxMessageItgServiceTest
    {
        private readonly Mock<IOutboxMessageRepository> _outboxRepository;
        private readonly Mock<IPublisher> _publisher;
        private readonly Mock<ILogWriter> _logWriter;

        public OutboxMessageItgServiceTest()
        {
            _outboxRepository = new(MockBehavior.Strict);
            _publisher = new(MockBehavior.Strict);
            _logWriter = new(MockBehavior.Strict);
        }

        [Fact]
        public async Task ExecuteAsync_GivenMessagesAvailable_ThenPublishMessagesAndUpdateMessagesStatus()
        {
            // Arrange
            var (sut, outboxRepository, publisher, _) = new OutboxMessageItgServiceMockBuilder()
                .WithMessagesAvailable()
                .BuildWithMocks();

            // Act
            await sut.ExecuteAsync();

            // Assert
            publisher.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<Guid>()), Times.AtLeastOnce);
            outboxRepository.Verify(x => x.UpdateMessageStateToSendToQueueAsync(It.IsAny<Guid>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ExecuteAsync_WhenHasNotMessagesAvailable_ThenWriteLogAndReturn()
        {
            // Arrange
            var (sut, outboxRepository, _, logWriter) = new OutboxMessageItgServiceMockBuilder()
                .WithNotMessagesAvailable()
                .BuildWithMocks();

            // Act
            await sut.ExecuteAsync();

            // Assert
            Mock.VerifyAll(outboxRepository, logWriter);
        }

        [Theory, AutoData]
        public async Task ExecuteAsync_WhenPublishMessageFails_ThenContinue(Exception exception)
        {
            // Arrange
            var (sut, outboxRepository, publisher, logWriter) = new OutboxMessageItgServiceMockBuilder()
                .WithPublisherFails(exception)
                .BuildWithMocks();

            // Act
            await sut.ExecuteAsync();

            // Assert
            publisher.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<Guid>()), Times.AtLeastOnce);
            outboxRepository.Verify(x => x.UpdateMessageStateToSendToQueueAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
