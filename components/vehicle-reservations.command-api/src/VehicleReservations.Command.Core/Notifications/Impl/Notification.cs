using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VehicleReservations.Command.Core.Notifications
{
    internal class Notification : INotification
    {
        private readonly Queue<(string, int)> _notifications = new();

        public void Add(string message, int statusCode = 400) =>
            _notifications.Enqueue((message, statusCode));

        public void Add(Exception exception, int statusCode = 500) =>
            _notifications.Enqueue((exception.Message, statusCode));

        public bool Any() => _notifications.Count > 0;

        public string GetSummary() =>
            _notifications.Aggregate(
                new StringBuilder(),
                (sb, message) => sb.AppendLine(message.Item1))
            .ToString();

        public string GetErrorStatus() =>
            _notifications.Peek().Item2.ToString();
    }
}
