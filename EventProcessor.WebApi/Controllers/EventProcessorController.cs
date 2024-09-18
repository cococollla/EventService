using EventProcessor.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared.Enums;
using Shared.Models;

namespace EventProcessor.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для управления инцидентами в службе Event Processor.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EventProcessorController : ControllerBase
    {
        private readonly IEventProcessorService _eventProcessorService;
        private readonly IMemoryCache _memoryCache;

        private const string casheKey = "EVENTS";

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="eventProcessorService">Сервис для обработки событий и создания инцидентов.</param>
        /// <param name="memoryCache">Кэш.</param>
        public EventProcessorController(IEventProcessorService eventProcessorService, IMemoryCache memoryCache)
        {
            _eventProcessorService = eventProcessorService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Получает событие созданное в ручную и обрабатывает его для создания инцидента.
        /// </summary>
        /// <param name="newEvent">Событие для обработки.</param>
        /// <param name="cancellationToke"»>Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        [HttpPost("AutoReceive")]
        public async Task<IActionResult> AutoReceiveEventProcessingAsync([FromBody] Event newEvent, CancellationToken cancellationToken)
        {
            try
            {
                if (newEvent.Type == EventTypeEnum.Type1 || newEvent.Type == EventTypeEnum.Type2)
                {
                    var @event = new Models.Event()
                    {
                        Id = newEvent.Id,
                        Time = newEvent.Time,
                        Type = newEvent.Type,
                    };

                    EventQueueManager.EnqueueEvent(@event);
                }

                return Ok("Событие успешно обработано.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает список созданных инцидентов с опциональной сортировкой и пагинацией.
        /// </summary>
        /// <param name="sortOrder">Порядок, в котором следует сортировать инциденты.</param>
        /// <param name="pageNumber">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        [HttpGet("Incidents")]
        public async Task<IActionResult> GetIncidentsAsync(
            [FromQuery] string sortOrder = "asc",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var incidents = await _eventProcessorService.GetIncidentsAsync(sortOrder, pageNumber, pageSize, cancellationToken);
                return Ok(incidents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
