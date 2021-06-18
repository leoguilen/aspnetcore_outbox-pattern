using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Broker.Configurations;
using RabbitMQ.Client;

namespace OutboxMessage.Itg.Infra.Broker.RabbitMQ
{
    internal class RabbitPublisher : IPublisher
    {
        private readonly ILogWriter _logWriter;
        private readonly IRabbitConnection _connection;
        private readonly RabbitConfiguration _rabbitConfig;

        public RabbitPublisher(
            ILogWriter logWriter,
            IRabbitConnection connection,
            RabbitConfiguration rabbitConfig)
        {
            _logWriter = logWriter;
            _connection = connection;
            _rabbitConfig = rabbitConfig;
        }

        public Task Publish(object message, Guid correlationId)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                BasicPublish(message, correlationId);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logWriter.Error(
                    message: $"Error publishing message in queue {_rabbitConfig.QueueName}",
                    data: message,
                    ex: ex);
                throw;
            }
        }

        private void BasicPublish(object message, Guid correlationId)
        {
            using var channel = CreateNewChannel();

            _logWriter.Info($"Publishing message to RabbitMQ in Queue: {_rabbitConfig.QueueName}");

            var basicProps = channel.CreateBasicProperties();
            basicProps.Persistent = true;
            basicProps.Expiration = "300000";
            basicProps.Headers = new Dictionary<string, object>()
            {
                { "x-correlation_id", correlationId.ToString() }
            };

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: _rabbitConfig.QueueName,
                mandatory: true,
                basicProperties: basicProps,
                body: Encoding.UTF8.GetBytes((string)message));

            channel.WaitForConfirmsOrDie();
        }

        private IModel CreateNewChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            return _connection.CreateModel(string.Empty);
        }
    }
}
