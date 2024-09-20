using EventProcessor.WebApi.Data;
using EventProcessor.WebApi.Data.Models;
using EventProcessor.WebApi.Enums;
using EventProcessor.WebApi.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace EventProcessor.WebApi.Services.Implementations
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

            query = sortOrder switch
            {
                "desc" => query.OrderByDescending(i => i.Time),
                _ => query.OrderBy(i => i.Time),
            };

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.Include(i => i.Events).ToListAsync(cancellationToken);
        }

        private async Task HandleCreateIncidentAsync(Event currentEvent, CancellationToken cancellationToken, Event prevEvent = null)
        {
            try
            {
                if (_pendingEventType2 != null
                    && currentEvent.Time - _pendingEventType2.Time <= _compositeTemplateTimeLimit
                    && currentEvent.Type == EventTypeEnum.Type1)
                {
                    var incidentId = await CreateIncidentAsync(IncidentTypeEnum.Type2, cancellationToken);

                    var eventCompositeId = await CreateEventAsync(_pendingEventType2, incidentId, cancellationToken);
                    var eventSimpleId = await CreateEventAsync(currentEvent, incidentId, cancellationToken);

                    _logger.LogInformation($"Создан составной инцидент {incidentId} с событиями {eventSimpleId} и {eventCompositeId}");
                }
                else if (currentEvent != null && currentEvent.Type == EventTypeEnum.Type1)
                {
                    var incidentId = await CreateIncidentAsync(IncidentTypeEnum.Type1, cancellationToken);
                    var eventSimpleId = await CreateEventAsync(currentEvent, incidentId, cancellationToken);

                    _logger.LogInformation($"Создан простой инцидент {incidentId} для события {eventSimpleId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при создании инцидента: {ex.Message}");
            }
        }

        private async Task<Guid> CreateIncidentAsync(IncidentTypeEnum type, CancellationToken cancellationToken)
        {
            var incident = new Incident
            {
                Id = Guid.NewGuid(),
                Type = type,
                Time = DateTime.UtcNow
            };

            _dbContext.Incidents.Add(incident);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return incident.Id;
        }

        private async Task<Guid> CreateEventAsync(Event newEvent, Guid incidentId, CancellationToken cancellationToken)
        {
            newEvent.IncidentId = incidentId;

            _dbContext.Events.Add(newEvent);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newEvent.Id;
        }
    }
}
