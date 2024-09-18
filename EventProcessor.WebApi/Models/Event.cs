using Shared.Enums;

namespace EventProcessor.WebApi.Models
{
    /// <summary>
    /// Представляет событие.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Тип события (например, Type1, Type2, и тд.).
        /// </summary>
        public EventTypeEnum Type { get; set; }

        /// <summary>
        /// Дата создания события.
        /// </summary>
        public DateTime Time { get; set; }

        // Внешний ключ для инцидента.
        public Guid? IncidentId { get; set; }
        public Incident Incident { get; set; }
    }
}
