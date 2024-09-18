using EventProcessor.WebApi.Models;

namespace EventProcessor.WebApi.Services
{
    /// <summary>
    /// Определяет методы для обработки событий и создания инцидентов.
    /// </summary>
    public interface IEventProcessorService
    {
        /// <summary>
        /// Обрабатывает событие и создает инцидент, если применимо.
        /// </summary>
        /// <param name="newEvent">Событие для обработки.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>>
        Task ProcessEventAsync(Event newEvent, CancellationToken cancellationToken);

        /// <summary>
        /// Получает список созданных инцидентов.
        /// </summary>
        /// <param name="sortOrder">Порядок, в котором следует сортировать инциденты.</param>
        /// <param name="pageNumber">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task<IEnumerable<Incident>> GetIncidentsAsync(string sortOrder, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
