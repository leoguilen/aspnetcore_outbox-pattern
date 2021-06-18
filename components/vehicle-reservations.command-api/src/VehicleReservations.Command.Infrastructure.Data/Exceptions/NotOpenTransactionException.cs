using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VehicleReservations.Command.Infrastructure.Data.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal class NotOpenTransactionException : Exception
    {
        public NotOpenTransactionException()
        {
        }

        public NotOpenTransactionException(string message) : base(message)
        {
        }

        public NotOpenTransactionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotOpenTransactionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
