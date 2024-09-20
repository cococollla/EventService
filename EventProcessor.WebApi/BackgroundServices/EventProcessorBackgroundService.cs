using EventProcessor.WebApi.Data.Models;
using EventProcessor.WebApi.Services.Contracts;
using System.Threading.Channels;

namespace EventProcessor.WebApi.BackgroundServices
{
    /// <summary>
    /// Фоновая служба, которая обрабатывает события и создает на их основе инциденты.
    /// </summary>
    public class EventProcessorBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ChannelReader<Event> _channelReader;
        private readonly ILogger<EventProcessorBackgroundService> _logger;

        public EventProcessorBackgroundService(
            IServiceScopeFactory serviceScopeFactory,
            Channel<Event> channel,
            ILogger<EventProcessorBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _channelReader = channel.Reader;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (await _channelReader.WaitToReadAsync(cancellationToken))
            {
                try
                {
                    var @event = await _channelReader.ReadAsync(cancellationToken);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var eventProcessorService = scope.ServiceProvider.GetRequiredService<IEventProcessorService>();

                        await eventProcessorService.ProcessEventAsync(@event, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при обработке события");
                }
            }
        }
    }
}
