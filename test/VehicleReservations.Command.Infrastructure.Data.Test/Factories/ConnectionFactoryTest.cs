using System;
using System.Data.SqlClient;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using VehicleReservations.Command.Infrastructure.Data.Configurations;
using VehicleReservations.Command.Infrastructure.Data.Factories;
using VehicleReservations.Command.Infrastructure.Data.Providers;
using Xunit;

namespace VehicleReservations.Command.Infrastructure.Data.Test.Context
{
    [Trait("Unit", nameof(ConnectionFactory))]
    public class ConnectionFactoryTest
    {
        private const string SqlServerProviderName = "SQLServer";

        private readonly Mock<IConnectionProvider> _provider;

        public ConnectionFactoryTest()
        {
            _provider = new(MockBehavior.Strict);
        }

        [Theory, AutoData]
        public void Constructor_GivenValidParameters_ThenInitializeNewInstance(Fixture fixture)
        {
            // Arrange
            var dbConfig = fixture.Create<DatabaseSettings>();

            // Act
            var result = new ConnectionFactory(_provider.Object, dbConfig);

            // Assert
            result.Should().NotBeNull();
            _provider.VerifyAll();
        }

        [Theory, AutoData]
        public void Constructor_GivenDbConfigurationIsNull_ThenThrowExpectedArgumentNullException(Fixture fixture)
        {
            // Arrange
            const string expectedExMsg = "Value cannot be null. (Parameter 'dbSettings')";
            fixture.Inject<DatabaseSettings>(null);
            var dbConfig = fixture.Create<DatabaseSettings>();

            // Act
            Action act = () => _ = new ConnectionFactory(_provider.Object, dbConfig);

            // Assert
            act.Should()
                .Throw<ArgumentNullException>()
                .WithMessage(expectedExMsg);
        }

        [Theory, AutoData]
        public void GetNewConnection_WhenCallWithValidConnectionStringAndProviderName_ReturnNewSqlServerConnection(Fixture fixture)
        {
            // Arrange
            var connectionString = fixture.Create("Data Source=");
            var dbConfig = new DatabaseSettings()
            {
                ProviderName = SqlServerProviderName,
                ConnectionString = connectionString
            };
            _provider
                .Setup(x => x.CreateConnection(SqlServerProviderName, dbConfig.ConnectionString))
                .Returns(new SqlConnection(dbConfig.ConnectionString));
            var sut = new ConnectionFactory(_provider.Object, dbConfig);

            // Act
            var result = sut.GetNewConnection();

            // Assert
            result.Should()
                .NotBeNull().And
                .BeOfType<SqlConnection>();
            _provider.VerifyAll();
        }
    }
}
