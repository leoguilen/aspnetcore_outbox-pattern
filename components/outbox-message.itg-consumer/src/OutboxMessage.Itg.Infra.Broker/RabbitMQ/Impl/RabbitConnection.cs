using System;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Broker.Configurations;
using RabbitMQ.Client;

namespace OutboxMessage.Itg.Infra.Broker.RabbitMQ
{
    internal class RabbitConnection : IRabbitConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogWriter _logWriter;

        private IConnection _connection;
        private bool _disposed;

        public RabbitConnection(
            IConnectionFactory connectionFactory,
            ILogWriter logWriter,
            RabbitConfiguration rabbitConfig)
        {
            _logWriter = logWriter;
            _connectionFactory = connectionFactory;

            Connect();
            DeclareQueue(rabbitConfig.QueueName);
        }

        public IModel Channel { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Connect()
        {
            try
            {
                _connection?.Dispose();
                _connection = _connectionFactory.CreateConnection();
                Channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                _logWriter.Fatal("error establishing connection", ex);
            }
        }

        private void DeclareQueue(string QueueName)
        {
            Channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);
        }

        ~RabbitConnection() => Dispose(false);

        private void Dispose(bool dispose)
        {
            if (_disposed)
            {
                return;
            }

            if (dispose)
            {
                _connection?.Dispose();
            }

            _disposed = true;
        }
    }
}
