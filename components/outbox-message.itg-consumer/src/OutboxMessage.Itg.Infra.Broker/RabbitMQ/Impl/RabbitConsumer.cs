using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Core.Interfaces.Services;
using OutboxMessage.Itg.Core.Models;
using OutboxMessage.Itg.Infra.Broker.Configurations;
using OutboxMessage.Itg.Infra.Broker.Helpers;
using RabbitMQ.Client.Events;

namespace OutboxMessage.Itg.Infra.Broker.RabbitMQ
{
    internal class RabbitConsumer : IConsumer
    {
        private readonly ILogWriter _logWriter;
        private readonly IRabbitConnection _connection;
        private readonly IMessageParser _parser;
        private readonly RabbitConfiguration _rabbitConfig;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitConsumer(
            ILogWriter logWriter,
            IRabbitConnection connection,
            IMessageParser parser,
            RabbitConfiguration rabbitConfig,
            IServiceScopeFactory scopeFactory)
        {
            _logWriter = logWriter;
            _connection = connection;
            _parser = parser;
            _rabbitConfig = rabbitConfig;
            _scopeFactory = scopeFactory;
        }

        public Task ConsumeAsync()
        {
            var consumer = new AsyncEventingBasicConsumer(_connection.Channel);
            consumer.Received += async (_, e) => await OnReceive(e);

            _connection.Channel.BasicConsume(
                queue: _rabbitConfig.QueueName,
                autoAck: false,
                consumerTag: _rabbitConfig.QueueName,
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: consumer);
            _logWriter.Info("consumer started");

            return Task.CompletedTask;
        }

        private async Task OnReceive(BasicDeliverEventArgs args)
        {
            var correlationId = _parser.ParseCorrelationId(args);

            try
            {
                await HandleMessage(args, correlationId);
            }
            catch (Exception ex)
            {
                HandleConsumeError(args, correlationId, ex);
                throw;
            }
        }

        private async Task HandleMessage(
            BasicDeliverEventArgs args,
            Guid correlationId)
        {
            using var scope = _scopeFactory.CreateScope();

            var message = GetValidMessage(args);
            var service = scope.ServiceProvider.GetRequiredService<IOutboxMessageItgService>();

            await service.ExecuteAsync(message);

            _connection.Channel.BasicAck(args.DeliveryTag, false);
            _logWriter.CorrelationId = correlationId;
            _logWriter.Info("message consumed successfully", message);
        }

        private VehicleReservation GetValidMessage(BasicDeliverEventArgs args) =>
            _parser.ParseMessage<VehicleReservation>(args);

        private void HandleConsumeError(
            BasicDeliverEventArgs args,
            Guid correlationId,
            Exception ex)
        {
            var body = args.Body.ToArray();
            var payload = Encoding.UTF8.GetString(body);

            _connection.Channel.BasicNack(args.DeliveryTag, false, true);
            _logWriter.CorrelationId = correlationId;
            _logWriter.Error("error consuming message", payload, ex);
        }
    }
}
