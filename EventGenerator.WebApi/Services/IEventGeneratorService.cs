namespace EventGenerator.WebApi.Services
{
    /// <summary>
    /// Определяет методы для генерации и отправки событий.
    /// </summary>
    public interface IEventGeneratorService
    {
        /// <summary>
        /// Генерирует событие и отправляет его в EventProcessor.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Задача, представляющая асинхронную операцию.</returns>
        Task GenerateAndSendEventAsync(CancellationToken cancellationToken);

    }
}
