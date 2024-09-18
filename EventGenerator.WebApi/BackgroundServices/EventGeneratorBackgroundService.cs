using EventGenerator.WebApi.Services;

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
        /// <param name="eventGeneratorService">Сервис для генерации и отправки событий.</param>
        /// <param name="logger">Логгер.</param>
        public EventGeneratorBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<EventGeneratorBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var eventGeneratorService = scope.ServiceProvider.GetRequiredService<IEventGeneratorService>();

                    await eventGeneratorService.GenerateAndSendEventAsync(stoppingToken);
                }

                await Task.Delay(_random.Next(0, 2000), stoppingToken);
            }
        }
    }
}
