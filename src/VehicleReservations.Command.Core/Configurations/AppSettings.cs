namespace VehicleReservations.Command.Core.Configurations
{
    public record AppSettings
    {
        public string Application { get; init; }

        public string Version { get; init; }

        public bool UseSwagger { get; init; }
    }
}
