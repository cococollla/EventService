using EventProcessor.WebApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models;
using System.Threading.Channels;

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

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="eventProcessorService">Сервис для обработки событий и создания инцидентов.</param>
        public EventProcessorController(IEventProcessorService eventProcessorService)
        {
            _eventProcessorService = eventProcessorService;
        }

        /// <summary>
        /// Получает событие созданное в ручную и обрабатывает его для создания инцидента.
        /// </summary>
        /// <param name="newEvent">Событие для обработки.</param>
        /// <param name="cancellationToke"»>Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        [HttpPost("AutoReceive")]
        public async Task<IActionResult> AutoReceiveEventProcessingAsync(
            [FromBody] Event newEvent,
            [FromServices] Channel<Data.Models.Event> channelWriter,
            CancellationToken cancellationToken)
        {
            try
            {
                if (newEvent.Type == EventTypeEnum.Type1 || newEvent.Type == EventTypeEnum.Type2)
                {
                    var @event = new Data.Models.Event()
                    {
                        Id = newEvent.Id,
                        Time = newEvent.Time,
                        Type = newEvent.Type,
                    };

                    await channelWriter.Writer.WriteAsync(@event, cancellationToken);
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
