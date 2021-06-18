using System;

namespace VehicleReservations.Command.Core.Notifications
{
    public interface INotification
    {
        void Add(string message, int statusCode = 400);

        void Add(Exception exception, int statusCode = 500);

        bool Any();

        string GetSummary();

        string GetErrorStatus();
    }
}
