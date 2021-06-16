using System;
using System.Data;

namespace VehicleReservations.Command.Core.Interfaces.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
