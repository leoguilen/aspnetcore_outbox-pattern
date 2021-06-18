using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Core.Services.Services;

namespace OutboxMessage.Itg.Core.Services.Test.Commons.Builders
{
    internal class OutboxMessageItgServiceMockBuilder
    {
        private readonly IFixture _fixture;
        private readonly Mock<IOutboxMessageRepository> _outboxRepository;
        private readonly Mock<IPublisher> _publisher;
        private readonly Mock<ILogWriter> _logWriter;

        public OutboxMessageItgServiceMockBuilder()
        {
            _fixture = new Fixture();
            _outboxRepository = new(MockBehavior.Strict);
            _publisher = new(MockBehavior.Strict);
            _logWriter = new(MockBehavior.Strict);
        }

        public OutboxMessageItgServiceMockBuilder WithNotMessagesAvailable()
        {
            _fixture.Inject<IAsyncEnumerable<Models.OutboxMessage>>(null);
            var nullMessages = _fixture.Create<IAsyncEnumerable<Models.OutboxMessage>>();

            _outboxRepository
                .Setup(x => x.GetAllMessagesAvailableAsync())
                .ReturnsAsync(nullMessages);
            _logWriter
                .Setup(x => x.Info("Hasn't available messages to process", null, null));

            return this;
        }

        public OutboxMessageItgServiceMockBuilder WithMessagesAvailable()
        {
            _fixture.Inject(_fixture.Create<IEnumerable<Models.OutboxMessage>>().ToAsyncEnumerable());
            var messages = _fixture.Create<IAsyncEnumerable<Models.OutboxMessage>>();

            _outboxRepository
                .Setup(x => x.GetAllMessagesAvailableAsync())
                .ReturnsAsync(messages);
            _publisher
                .Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
            _outboxRepository
                .Setup(x => x.UpdateMessageStateToSendToQueueAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            return this;
        }

        public OutboxMessageItgServiceMockBuilder WithPublisherFails(Exception exception)
        {
            _fixture.Inject(_fixture.Create<IEnumerable<Models.OutboxMessage>>().ToAsyncEnumerable());
            var messages = _fixture.Create<IAsyncEnumerable<Models.OutboxMessage>>();

            _outboxRepository
                .Setup(x => x.GetAllMessagesAvailableAsync())
                .ReturnsAsync(messages);
            _publisher
                .Setup(x => x.Publish(It.IsAny<object>(), It.IsAny<Guid>()))
                .ThrowsAsync(exception);
            _logWriter
                .Setup(x => x.Error(exception.Message, It.IsAny<object>(), exception));

            return this;
        }

        public OutboxMessageItgService Build() => new(
            _outboxRepository.Object,
            _publisher.Object,
            _logWriter.Object);

        public (OutboxMessageItgService, Mock<IOutboxMessageRepository>, Mock<IPublisher>, Mock<ILogWriter>) BuildWithMocks() => (
            new(
                _outboxRepository.Object,
                _publisher.Object,
                _logWriter.Object),
            _outboxRepository,
            _publisher,
            _logWriter);
    }
}
