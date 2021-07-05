using System;
using System.Diagnostics.CodeAnalysis;

namespace OutboxMessage.Itg.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string message) : base(message)
        {
        }

        public CoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
