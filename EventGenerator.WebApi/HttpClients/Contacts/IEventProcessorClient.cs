using Shared.Models;

namespace EventGenerator.WebApi.HttpClients.Contacts
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
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task SendEventAsync(Event newEvent, CancellationToken cancellationToken);
    }
}
