using System;
using RabbitMQ.Client;

namespace OutboxMessage.Itg.Infra.Broker.RabbitMQ
{
    public interface IRabbitConnection : IDisposable
    {
        public IModel Channel { get; }
    }
}
