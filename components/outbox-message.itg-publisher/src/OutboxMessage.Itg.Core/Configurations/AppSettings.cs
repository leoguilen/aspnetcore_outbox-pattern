namespace OutboxMessage.Itg.Core.Configurations
{
    public record AppSettings
    {
        public string Application { get; init; }

        public string Version { get; init; }
    }
}
