using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using VehicleReservations.Command.Infrastructure.Data.Providers.Impl;
using Xunit;

namespace VehicleReservations.Command.Infrastructure.Data.Test.Providers
{
    [Trait("Unit", nameof(ConnectionProvider))]
    public class ConnectionProviderTest
    {
        [Theory, InlineData("SQLServer", typeof(SqlConnection))]
        public void CreateConnection_GivenValidProviderName_ThenReturnProviderConnection(string providerName, Type providerType)
        {
            // Arrange
            var connectionString = new Fixture().Create("Data Source=");
            var sut = new ConnectionProvider();

            // Act
            var result = sut.CreateConnection(providerName, connectionString);

            // Assert
            result.Should().BeOfType(providerType);
            result.ConnectionString.Should().Be(connectionString);
        }

        [Theory, AutoData]
        public void CreateConnection_GivenInvalidProviderName_ThenThrowExpectedKeyNotFoundException(string providerName, string connectionString)
        {
            // Arrange
            var sut = new ConnectionProvider();

            // Act
            Func<IDbConnection> func = () => sut
                .CreateConnection(providerName, connectionString);

            // Assert
            func.Should().Throw<KeyNotFoundException>();
        }
    }
}
