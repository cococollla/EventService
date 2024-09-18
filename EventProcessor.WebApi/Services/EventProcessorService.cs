using EventProcessor.WebApi.Data;
using EventProcessor.WebApi.Enums;
using EventProcessor.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace EventProcessor.WebApi.Services
{
    /// <summary>
    /// Предоставляет функциональность для обработки событий и создания инцидентов.
    /// </summary>
    public class EventProcessorService : IEventProcessorService
    {
        private readonly ProcessorDbContext _dbContext;
        private readonly ILogger<EventProcessorService> _logger;
        private readonly TimeSpan _compositeTemplateTimeLimit = TimeSpan.FromSeconds(20);
        private static Event _pendingEventType2;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных.</param>
        /// <param name="logger">Логгер.</param>
        public EventProcessorService(ProcessorDbContext dbContext, ILogger<EventProcessorService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task ProcessEventAsync(Event newEvent, CancellationToken cancellationToken)
        {
            if (newEvent.Type == EventTypeEnum.Type2)
            {
                _pendingEventType2 = newEvent;
            }

            await HandleCreateIncidentAsync(newEvent, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Incident>> GetIncidentsAsync(string sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _dbContext.Incidents.AsQueryable();

            // Применяем сортировку.
            query = sortOrder switch
            {
                "desc" => query.OrderByDescending(i => i.Time),
                _ => query.OrderBy(i => i.Time),
            };

            // Применяем пагинацию.
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.Include(i => i.Events).ToListAsync(cancellationToken);
        }

        private async Task HandleCreateIncidentAsync(Event currentEvent, CancellationToken cancellationToken, Event prevEvent = null)
        {
            try
            {
                if (_pendingEventType2 != null
                    && currentEvent.Time.Add(_compositeTemplateTimeLimit) >= _pendingEventType2.Time
                    && currentEvent.Type == EventTypeEnum.Type1)
                {
                    var incidentId = await CreateIncidentAsync(_pendingEventType2, cancellationToken);

                    _logger.LogInformation($"Создан составной инцидент {incidentId} для события {currentEvent.Id}");
                }
                else if (currentEvent != null && currentEvent.Type == EventTypeEnum.Type1)
                {
                    var incidentId = await CreateIncidentAsync(currentEvent, cancellationToken);

                    _logger.LogInformation($"Создан простой инцидент {incidentId} для события {currentEvent.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при создании инцидента: {ex.Message}");
            }
        }

        private async Task<Guid> CreateIncidentAsync(Event newEvent, CancellationToken cancellationToken)
        {
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Type = newEvent.Type == EventTypeEnum.Type1 ? IncidentTypeEnum.Type1 : IncidentTypeEnum.Type2,
                Time = DateTime.UtcNow
            };

            _dbContext.Incidents.Add(incident);
            newEvent.IncidentId = incident.Id;
            _dbContext.Events.Add(newEvent);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return incident.Id;
        }
    }
}
