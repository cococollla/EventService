using Shared.Enums;

namespace Shared.Models
{
    /// <summary>
    /// Представляет событие, которое генерируется автоматически или в ручную.
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
    }
}
