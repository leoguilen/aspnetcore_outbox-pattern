using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OutboxMessage.Itg.Core.Interfaces.Infrastructure
{
    public interface IOutboxMessageRepository
    {
        Task<IAsyncEnumerable<Models.OutboxMessage>> GetAllMessagesAvailableAsync();

        Task UpdateMessageStateToSendToQueueAsync(Guid id);
    }
}
