using Shared.Models;

namespace EventGenerator.WebApi.Services.Contracts
{
    /// <summary>
    /// Определяет методы для генерации и отправки событий.
    /// </summary>
    public interface IEventGeneratorService
    {
        /// <summary>
        /// Генерирует событие.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Созданное событие.</returns>
        Task<Event> GenerateEventAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Отправляет событие.
        /// </summary>
        /// <param name="newEvent">Событие для отправки.</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Задача, представляющая асинхронную отправку события.</returns>
        Task SendEventToProcessorAsync(Event newEvent, CancellationToken cancellationToken);
    }
}
