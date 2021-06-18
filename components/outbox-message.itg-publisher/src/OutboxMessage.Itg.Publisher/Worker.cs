using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Core.Interfaces.Services;

namespace OutboxMessage.Itg.Publisher
{
    internal class Worker : BackgroundService
    {
        private const int ExecutionDelay = 30000;

        private readonly IOutboxMessageItgService _outboxService;
        private readonly ILogWriter _logWriter;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public Worker(
            IOutboxMessageItgService outboxService,
            ILogWriter logWriter,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _outboxService = outboxService;
            _logWriter = logWriter;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(ExecutionDelay, stoppingToken);
                _logWriter.Info($"Worker running at: {DateTimeOffset.Now}");

                try
                {
                    await _outboxService.ExecuteAsync();
                }
                catch (Exception)
                {
                    _hostApplicationLifetime.StopApplication();
                }
            }
        }
    }
}
