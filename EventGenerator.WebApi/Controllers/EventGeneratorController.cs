using EventGenerator.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models;

namespace EventGenerator.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventGeneratorController : ControllerBase
    {
        private readonly IEventGeneratorService _eventGeneratorService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="eventGeneratorService">Сервис для генерации и отправки событий.</param>
        public EventGeneratorController(IEventGeneratorService eventGeneratorService)
        {
            _eventGeneratorService = eventGeneratorService;
        }

        /// <summary>
        /// Вручную генерирует событие и отправляет его в процессор событий EventProcessor.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        [HttpPost("GenerateRandomEvent")]
        public async Task<IActionResult> GenerateEventAsync()
        {
            try
            {
                var cancellationToken = new CancellationToken();
                await _eventGeneratorService.GenerateAndSendEventAsync(cancellationToken);

                return Ok("Событие создано и успешно отправлено.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Вручную генерирует событие типа 1.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        [HttpPost("GenerateEventType1")]
        public async Task<IActionResult> GenerateEvent1Async()
        {
            try
            {
                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    Type = EventTypeEnum.Type1,
                    Time = DateTime.UtcNow
                };

                return Ok(newEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Вручную генерирует событие типа 2.
        /// </summary>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        [HttpPost("GenerateEventType2")]
        public async Task<IActionResult> GenerateEvent2Async()
        {
            try
            {
                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    Type = EventTypeEnum.Type2,
                    Time = DateTime.UtcNow
                };

                return Ok(newEvent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
