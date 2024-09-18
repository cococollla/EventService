using EventProcessor.WebApi.Services;

namespace EventProcessor.WebApi.BackgroundServices
{
    /// <summary>
    /// Фоновая служба, которая обрабатывает события и создает на их основе инциденты.
    /// </summary>
    public class EventProcessorBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EventProcessorBackgroundService> _logger;
        private readonly TimeSpan _compositeTemplateTimeLimit = TimeSpan.FromSeconds(20);

        public EventProcessorBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<EventProcessorBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (EventQueueManager.TryDequeueEvent(out var @event))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var eventProcessorService = scope.ServiceProvider.GetRequiredService<IEventProcessorService>();

                        await eventProcessorService.ProcessEventAsync(@event, stoppingToken);
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
