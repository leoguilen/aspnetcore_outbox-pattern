using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.Core.Enums;
using VehicleReservations.Command.FunctionalTest.Fixtures.DataFixture;
using Xunit;

namespace VehicleReservations.Command.FunctionalTest.Endpoints
{
    [Trait("Endpoint", "RenewReserveEndpoint")]
    public class RenewReserveEndpointTest :
        IClassFixture<CustomWebApplicationFactory>,
        IDisposable
    {
        private const string RenewReserveRequestUri = "api/v1/reservations/{0}/renew/{1}";

        private readonly HttpClient _httpClient;
        private readonly SqLiteDatabaseFixture _databaseFixture;

        // Setup
        public RenewReserveEndpointTest(CustomWebApplicationFactory appFactory)
        {
            _httpClient = appFactory.CreateClient();
            _databaseFixture = appFactory.Services.GetRequiredService<SqLiteDatabaseFixture>();
        }

        // Teardown
        public void Dispose() => _databaseFixture.DropDatabase();

        [Theory, AutoData]
        public async Task RenewReserveEndpoint_GivenExistentReserve_ThenRenewReserveAndReturnStatus200OK(Fixture fixture)
        {
            // Arrange
            var reserveId = Guid.NewGuid();
            var days = fixture.Create<int>();
            var actualExpireDate = AddReserve(fixture, reserveId);
            var expectedExpireDate = actualExpireDate.AddDays(days);

            // Act
            var result = await _httpClient.PatchAsync(
                requestUri: string.Format(RenewReserveRequestUri, reserveId, days), null);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            _databaseFixture.IsRenewedReserve(reserveId, expectedExpireDate);
        }

        [Theory, AutoData]
        public async Task RenewReserveEndpoint_GivenNonexistentReserve_ThenReturnStatus404NotFound(Fixture fixture)
        {
            // Arrange
            var reserveId = Guid.NewGuid();
            var days = fixture.Create<int>();

            // Act
            var result = await _httpClient.PatchAsync(
                requestUri: string.Format(RenewReserveRequestUri, reserveId, days), null);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private DateTime AddReserve(Fixture fixture, Guid reserveId)
        {
            var reservedAt = fixture.Create<DateTime>();
            var obj = new
            {
                ReserveId = reserveId,
                VehicleId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                ReservedAt = reservedAt,
                ReservationExpiresOn = reservedAt.AddDays(fixture.Create<byte>()),
                Value = fixture.Create<decimal>(),
                Status = (int)fixture.Create<ReserveStatus>(),
            };

            _databaseFixture.InsertReserve(obj);

            return obj.ReservationExpiresOn;
        }
    }
}
