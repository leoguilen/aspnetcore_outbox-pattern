using System;
using System.Threading.Tasks;

namespace OutboxMessage.Itg.Core.Interfaces.Infrastructure
{
    public interface IPublisher
    {
        Task Publish(object message, Guid correlationId);
    }
}
