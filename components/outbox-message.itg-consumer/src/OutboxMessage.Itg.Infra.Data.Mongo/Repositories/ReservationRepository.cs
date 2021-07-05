using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Core.Models;
using OutboxMessage.Itg.Infra.Data.Mongo.Models;

namespace OutboxMessage.Itg.Infra.Data.Mongo.Repositories
{
    internal class ReservationRepository : IReservationRepository
    {
        private const string ReservationsCollectionName = "RESERVATIONS";

        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<ReservationDocument> _collection;

        public ReservationRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = _database.GetCollection<ReservationDocument>(ReservationsCollectionName);
        }

        public async Task PersistAsync(VehicleReservation reservation) =>
            await _collection.UpdateOneAsync(
                Builders<ReservationDocument>.Filter.Eq(x => x.ReserveId, reservation.ReserveId),
                Builders<ReservationDocument>.Update
                    .Set(x => x.ReserveId, reservation.ReserveId)
                    .Set(x => x.VehicleId, reservation.VehicleId)
                    .Set(x => x.CustomerId, reservation.CustomerId)
                    .Set(x => x.ReservedAt, reservation.ReservedAt)
                    .Set(x => x.ReservationExpiresOn, reservation.ReservationExpiresOn)
                    .Set(x => x.Value, reservation.Value)
                    .Set(x => x.Status, (int)reservation.Status),
                new UpdateOptions() { IsUpsert = true },
                CancellationToken.None);
    }
}
