using Shared.Models;

namespace EventGenerator.WebApi.HttpClients
{
    /// <summary>
    /// Определяет методы для отправки событий в EventProcessor.
    /// </summary>
    public interface IEventProcessorClient
    {
        /// <summary>
        /// Отправляет событие в службу EventProcessor.
        /// </summary>
        /// <param name="newEvent">Событие, которое будет отправлено.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task SendEventAsync(Event newEvent);
    }
}
