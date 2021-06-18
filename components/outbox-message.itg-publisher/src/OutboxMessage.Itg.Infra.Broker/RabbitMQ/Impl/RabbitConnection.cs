using System;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Broker.Configurations;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OutboxMessage.Itg.Infra.Broker.RabbitMQ
{
    internal class RabbitConnection : IRabbitConnection
    {
        private readonly RabbitConfiguration _rabbitConfig;
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
            _rabbitConfig = rabbitConfig;
            _connectionFactory = connectionFactory;
        }

        ~RabbitConnection() => Dispose(true);

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateModel(string exchangeName)
        {
            if (IsConnected is false)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            var channel = _connection.CreateModel();

            channel.QueueDeclare(
                queue: _rabbitConfig.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.ConfirmSelect();

            return channel;
        }

        public void TryConnect()
        {
            _connection?.Dispose();
            _connection = _connectionFactory.CreateConnection();

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                return;
            }

            _logWriter.Fatal("RabbitMQ connections could not be created and opened");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            TryConnect();
        }

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
