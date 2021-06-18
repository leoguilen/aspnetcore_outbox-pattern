using System.Data;

namespace VehicleReservations.Command.Infrastructure.Data.Factories
{
    public interface IConnectionFactory
    {
        IDbConnection GetNewConnection();
    }
}
