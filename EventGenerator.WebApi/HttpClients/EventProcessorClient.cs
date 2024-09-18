using Shared.Models;

namespace EventGenerator.WebApi.HttpClients
{
    /// <summary>
    /// Предоставляет функциональность для отправки событий в EventProcessor.
    /// </summary>
    public class EventProcessorClient : IEventProcessorClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="httpClient">HTTP-клиент, используемый для выполнения запросов к службе EventProcessor.</param>
        public EventProcessorClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc/>
        public async Task SendEventAsync(Event newEvent)
        {
            var response = await _httpClient.PostAsJsonAsync("api/EventProcessor/AutoReceive", newEvent);
            response.EnsureSuccessStatusCode();
        }
    }
}
