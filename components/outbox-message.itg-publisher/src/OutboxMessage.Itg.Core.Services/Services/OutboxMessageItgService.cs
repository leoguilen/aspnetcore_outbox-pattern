using System.Threading.Tasks;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Core.Interfaces.Services;

namespace OutboxMessage.Itg.Core.Services.Services
{
    internal class OutboxMessageItgService : IOutboxMessageItgService
    {
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IPublisher _publisher;
        private readonly ILogWriter _logWriter;

        public OutboxMessageItgService(
            IOutboxMessageRepository outboxRepository,
            IPublisher publisher,
            ILogWriter logWriter)
        {
            _outboxRepository = outboxRepository;
            _publisher = publisher;
            _logWriter = logWriter;
        }

        public async Task ExecuteAsync()
        {
            var messages = await _outboxRepository
                .GetAllMessagesAvailableAsync();

            if (messages is null)
            {
                _logWriter.Info("Hasn't available messages to process");
                return;
            }

            await foreach (var message in messages)
            {
                try
                {
                    await _publisher.Publish(message.Payload, message.CorrelationId);
                    await _outboxRepository.UpdateMessageStateToSendToQueueAsync(message.Id);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
