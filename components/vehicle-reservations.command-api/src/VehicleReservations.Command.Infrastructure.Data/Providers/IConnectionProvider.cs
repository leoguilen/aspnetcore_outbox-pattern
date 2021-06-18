using System.Data;

namespace VehicleReservations.Command.Infrastructure.Data.Providers
{
    internal interface IConnectionProvider
    {
        IDbConnection CreateConnection(string providerName, string connectionString);
    }
}
