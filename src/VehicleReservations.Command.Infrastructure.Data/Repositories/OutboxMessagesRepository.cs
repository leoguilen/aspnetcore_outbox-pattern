using System.Threading.Tasks;
using Dapper;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Infrastructure.Data.Repositories.Statements;

namespace VehicleReservations.Command.Infrastructure.Data.Repositories
{
    internal class OutboxMessagesRepository : IOutboxMessagesRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public OutboxMessagesRepository(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public async Task AddAsync(object message) =>
            await _unitOfWork.Connection.ExecuteAsync(
                sql: SqlStatements.InsertOutboxMessage,
                param: message,
                transaction: _unitOfWork.Transaction);
    }
}
