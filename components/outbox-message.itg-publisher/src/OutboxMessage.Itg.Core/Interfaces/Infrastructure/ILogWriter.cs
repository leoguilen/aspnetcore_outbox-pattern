using System;

namespace OutboxMessage.Itg.Core.Interfaces.Infrastructure
{
    public interface ILogWriter
    {
        void Info(string message, object data = default, Exception ex = default);

        void Warn(string message, object data = default, Exception ex = default);

        void Error(string message, object data = default, Exception ex = default);

        void Fatal(string message, object data = default, Exception ex = default);
    }
}
