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
    [Trait("Endpoint", "CancelReserveEndpoint")]
    public class CancelReserveEndpointTest :
        IClassFixture<CustomWebApplicationFactory>,
        IDisposable
    {
        private const string CancelReserveRequestUri = "api/v1/reservations/{0}/cancel";

        private readonly HttpClient _httpClient;
        private readonly SqLiteDatabaseFixture _databaseFixture;

        // Setup
        public CancelReserveEndpointTest(CustomWebApplicationFactory appFactory)
        {
            _httpClient = appFactory.CreateClient();
            _databaseFixture = appFactory.Services.GetRequiredService<SqLiteDatabaseFixture>();
        }

        // Teardown
        public void Dispose() => _databaseFixture.DropDatabase();

        [Theory, AutoData]
        public async Task CancelReserveEndpoint_GivenExistentReserve_ThenCancelReserveReturnStatus200OK(Fixture fixture)
        {
            // Arrange
            var reserveId = Guid.NewGuid();
            AddReserve(fixture, reserveId);

            // Act
            var result = await _httpClient.PatchAsync(string.Format(CancelReserveRequestUri, reserveId), null);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            _databaseFixture.IsCanceledReserve(reserveId).Should().BeTrue();
        }

        [Fact]
        public async Task CreateReserveEndpoint_GivenNonexistentReserve_ThenReturnStatus404NotFound()
        {
            // Arrange
            var reserveId = Guid.NewGuid();

            // Act
            var result = await _httpClient.PatchAsync(string.Format(CancelReserveRequestUri, reserveId), null);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private void AddReserve(Fixture fixture, Guid reserveId)
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
        }
    }
}
