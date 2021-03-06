using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client.Events;

namespace OutboxMessage.Itg.Infra.Broker.Helpers
{
    internal class MessageParser : IMessageParser
    {
        private const string HeaderCorrelationId = "x-correlation_id";

        public Guid ParseCorrelationId(BasicDeliverEventArgs args)
        {
            var headers = args.BasicProperties?.Headers;

            return headers is null
                ? Guid.NewGuid()
                : ExtractCorrelationId(headers);
        }

        public T ParseMessage<T>(BasicDeliverEventArgs args)
        {
            var payload = Encoding.UTF8.GetString(args.Body.ToArray());
            return JsonSerializer.Deserialize<T>(payload);
        }

        private static Guid ExtractCorrelationId(IDictionary<string, object> headers)
        {
            return headers.TryGetValue(HeaderCorrelationId, out var headerId)
                ? ExtractCorrelationId(headerId.ToString())
                : Guid.NewGuid();
        }

        private static Guid ExtractCorrelationId(string headerId)
        {
            return Guid.TryParse(headerId, out var guid)
                ? guid
                : Guid.NewGuid();
        }
    }
}
