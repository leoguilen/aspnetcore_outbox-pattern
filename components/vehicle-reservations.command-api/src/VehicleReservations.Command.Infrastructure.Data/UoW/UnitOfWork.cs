using System;
using System.Data;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Infrastructure.Data.Exceptions;
using VehicleReservations.Command.Infrastructure.Data.Factories;

namespace VehicleReservations.Command.Infrastructure.Data.UoW
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IConnectionFactory _connectionFactory;

        private IDbConnection _connection;

        private int _transactionCounter = 0;
        private bool _disposedValue;

        public UnitOfWork(IConnectionFactory connectionFactory) =>
            _connectionFactory = connectionFactory;

        public IDbConnection Connection =>
            _connection ??= _connectionFactory.GetNewConnection();

        public IDbTransaction Transaction { get; protected set; }

        public void BeginTransaction()
        {
            if (_transactionCounter == 0)
            {
                Connection.Open();
                Transaction = Connection.BeginTransaction();
            }

            _transactionCounter++;
        }

        public void Commit()
        {
            try
            {
                TryCommit();
            }
            catch (NotOpenTransactionException)
            {
                throw;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            if (Transaction is null)
            {
                return;
            }

            _transactionCounter = 0;
            Transaction.Rollback();

            ClearTransaction();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing && _connection is not null)
            {
                _connection.Close();
                _connection.Dispose();
            }

            _disposedValue = true;
        }

        private void TryCommit()
        {
            if (Transaction is null || _transactionCounter < 0)
            {
                throw new NotOpenTransactionException("Commit");
            }

            _transactionCounter--;
            if (_transactionCounter > 0)
            {
                return;
            }

            Transaction.Commit();

            ClearTransaction();
        }

        private void ClearTransaction()
        {
            Transaction.Dispose();
            Transaction = null;
        }
    }
}
