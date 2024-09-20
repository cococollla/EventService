using EventGenerator.WebApi.HttpClients.Contacts;
using EventGenerator.WebApi.Services.Contracts;
using Shared.Enums;
using Shared.Models;

namespace EventGenerator.WebApi.Services.Implementations
{
    /// <summary>
    /// Предоставляет функциональность для генерации событий и отправки их в EventProcessor.
    /// </summary>
    public class EventGeneratorService : IEventGeneratorService
    {
        private readonly IEventProcessorClient _eventProcessorClient;
        private readonly ILogger<EventGeneratorService> _logger;
        private readonly Random _random = new Random();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="eventProcessorClient">HTTP-клиент, используемый для отправки событий в EventProcessor</param>
        /// <param name="logger">Логгер.</param>
        public EventGeneratorService(IEventProcessorClient eventProcessorClient, ILogger<EventGeneratorService> logger)
        {
            _eventProcessorClient = eventProcessorClient;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Event> GenerateEventAsync(CancellationToken cancellationToken)
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Type = (EventTypeEnum)_random.Next(1, 5),
                Time = DateTime.UtcNow
            };
            _logger.LogInformation($"Создано событие {newEvent.Id} в {newEvent.Time}");

            return newEvent;
        }

        /// <inheritdoc/>
        public async Task SendEventToProcessorAsync(Event newEvent, CancellationToken cancellationToken)
        {
            try
            {
                await _eventProcessorClient.SendEventAsync(newEvent, cancellationToken);
                _logger.LogInformation($"Успешная отправка события {newEvent.Id}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Ошибка при отправке события {newEvent.Id}");
            }
        }
    }
}
