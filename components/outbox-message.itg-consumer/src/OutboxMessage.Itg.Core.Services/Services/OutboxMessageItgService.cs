using System;
using System.Threading.Tasks;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Core.Interfaces.Services;
using OutboxMessage.Itg.Core.Models;

namespace OutboxMessage.Itg.Core.Services.Services
{
    internal class OutboxMessageItgService : IOutboxMessageItgService
    {
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ILogWriter _logWriter;

        public OutboxMessageItgService(
            IOutboxMessageRepository outboxRepository,
            ILogWriter logWriter,
            IReservationRepository reservationRepository)
        {
            _outboxRepository = outboxRepository;
            _logWriter = logWriter;
            _reservationRepository = reservationRepository;
        }

        public async Task ExecuteAsync(VehicleReservation message)
        {
            try
            {
                await _reservationRepository.PersistAsync(message);
                //await _outboxRepository.UpdateMessageStateToCompletedAsync(message.ReserveId);

                _logWriter.Info("Message processed with successfully");
            }
            catch (Exception ex)
            {
                _logWriter.Error(ex.Message, message, ex);
                throw;
            }
        }
    }
}
