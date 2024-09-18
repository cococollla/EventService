using EventProcessor.WebApi.Enums;

namespace EventProcessor.WebApi.Models
{
    /// <summary>
    /// Представляет инцидент.
    /// </summary>
    public class Incident
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Тип инцидента.
        /// </summary>
        public IncidentTypeEnum Type { get; set; }

        /// <summary>
        /// Время создания.
        /// </summary>
        public DateTime Time { get; set; }

        // Навигационное свойство для события.
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
