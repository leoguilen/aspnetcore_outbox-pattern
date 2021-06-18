using System;
using RabbitMQ.Client;

namespace OutboxMessage.Itg.Infra.Broker.RabbitMQ
{
    public interface IRabbitConnection : IDisposable
    {
        bool IsConnected { get; }

        IModel CreateModel(string exchangeName);

        void TryConnect();
    }
}
