using System;
using System.Threading.Tasks;

namespace OutboxMessage.Itg.Core.Interfaces.Infrastructure
{
    public interface IOutboxMessageRepository
    {
        Task UpdateMessageStateToCompletedAsync(Guid reserveId);
    }
}
