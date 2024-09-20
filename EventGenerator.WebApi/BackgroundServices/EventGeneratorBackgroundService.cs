using EventGenerator.WebApi.Services.Contracts;

namespace EventGenerator.WebApi.HostedServices
{
    /// <summary>
    /// Фоновая служба для генерации и отправки событий.
    /// </summary>
    public class EventGeneratorBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<EventGeneratorBackgroundService> _logger;
        private readonly Random _random = new Random();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="serviceScopeFactory">Сервис для создания скоупа.</param>
        /// <param name="logger">Логгер.</param>
        public EventGeneratorBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<EventGeneratorBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var eventGeneratorService = scope.ServiceProvider.GetRequiredService<IEventGeneratorService>();

                        var newEvent = await eventGeneratorService.GenerateEventAsync(cancellationToken);
                        await eventGeneratorService.SendEventToProcessorAsync(newEvent, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при отправке события");
                }

                await Task.Delay(_random.Next(0, 2000), cancellationToken);
            }
        }
    }
}
