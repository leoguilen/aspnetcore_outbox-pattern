namespace OutboxMessage.Itg.Infra.Broker.Configurations
{
    internal record RabbitConfiguration
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }

        public string QueueName { get; set; }
    }
}
