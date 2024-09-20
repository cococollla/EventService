using EventGenerator.WebApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared.Enums;
using Shared.Models;
using System.Threading;

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
        /// <returns>Результат отправки события.</returns>
        [HttpPost("GenerateRandomEvent")]
        public async Task<IActionResult> GenerateEventAsync(CancellationToken cancellationToken)
        {
            try
            {
                var newEvent = await _eventGeneratorService.GenerateEventAsync(cancellationToken);
                await _eventGeneratorService.SendEventToProcessorAsync(newEvent, cancellationToken);

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
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>>Результат отправки события.</returns>
        [HttpPost("GenerateEventTypeOne")]
        public async Task<IActionResult> GenerateEventTypeOneAsync(CancellationToken cancellationToken)
        {
            try
            {
                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    Type = EventTypeEnum.Type1,
                    Time = DateTime.UtcNow
                };

                await _eventGeneratorService.SendEventToProcessorAsync(newEvent, cancellationToken);

                return Ok("Событие создано и успешно отправлено.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        /// <summary>
        /// Вручную генерирует событие типа 2.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>>Результат отправки события.</returns>
        [HttpPost("GenerateEventTypeTwo")]
        public async Task<IActionResult> GenerateEventTypeTwoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    Type = EventTypeEnum.Type2,
                    Time = DateTime.UtcNow
                };

                await _eventGeneratorService.SendEventToProcessorAsync(newEvent, cancellationToken);

                return Ok("Событие создано и успешно отправлено.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }
}
