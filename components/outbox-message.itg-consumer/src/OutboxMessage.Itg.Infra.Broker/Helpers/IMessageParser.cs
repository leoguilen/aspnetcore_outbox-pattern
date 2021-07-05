using System;
using RabbitMQ.Client.Events;

namespace OutboxMessage.Itg.Infra.Broker.Helpers
{
    public interface IMessageParser
    {
        T ParseMessage<T>(BasicDeliverEventArgs args);
        Guid ParseCorrelationId(BasicDeliverEventArgs args);
    }
}
