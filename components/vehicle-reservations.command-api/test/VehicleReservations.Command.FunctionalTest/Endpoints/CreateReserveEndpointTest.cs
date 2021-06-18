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
    [Trait("Endpoint", "CreateReserveEndpoint")]
    public class CreateReserveEndpointTest :
        IClassFixture<CustomWebApplicationFactory>,
        IDisposable
    {
        private const string CreateReserveRequestUri = "api/v1/reservations";

        private readonly HttpClient _httpClient;
        private readonly SqLiteDatabaseFixture _databaseFixture;

        // Setup
        public CreateReserveEndpointTest(CustomWebApplicationFactory appFactory)
        {
            _httpClient = appFactory.CreateClient();
            _databaseFixture = appFactory.Services.GetRequiredService<SqLiteDatabaseFixture>();
        }

        // Teardown
        public void Dispose() => _databaseFixture.DropDatabase();

        [Theory, AutoData]
        public async Task CreateReserveEndpoint_GivenValidRequestBody_ThenCreateReserveAndReturnStatus201Created(Fixture fixture)
        {
            // Arrange
            var reservedAt = fixture.Create<DateTime>();
            var requestBody = new
            {
                VehicleId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                ReservedAt = reservedAt,
                ReservationExpiresOn = reservedAt.AddDays(fixture.Create<byte>()),
                Value = fixture.Create<decimal>(),
                Status = (int)fixture.Create<ReserveStatus>(),
            };

            // Act
            var result = await _httpClient.PostAsJsonAsync(CreateReserveRequestUri, requestBody);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            _databaseFixture.ExistsReserveBy(requestBody.VehicleId, requestBody.CustomerId).Should().BeTrue();
        }

        [Theory, AutoData]
        public async Task CreateReserveEndpoint_GivenUnavailableVehicle_ThenReturnStatus422UnprocessableEntity(Fixture fixture)
        {
            // Arrange
            var reservedAt = fixture.Create<DateTime>();
            var requestBody = new
            {
                ReserveId = Guid.NewGuid(),
                VehicleId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                ReservedAt = reservedAt,
                ReservationExpiresOn = reservedAt.AddDays(fixture.Create<byte>()),
                Value = fixture.Create<decimal>(),
                Status = (int)fixture.Create<ReserveStatus>(),
            };
            _databaseFixture.InsertReserve(requestBody);

            // Act
            var result = await _httpClient.PostAsJsonAsync(CreateReserveRequestUri, requestBody);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }
    }
}
