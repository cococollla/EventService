using EventProcessor.WebApi.Enums;

namespace EventProcessor.WebApi.Data.Models
{
    /// <summary>
    /// Представляет инцидент.
    /// </summary>
    public class Incident
    {
        public Guid Id { get; set; }

        public IncidentTypeEnum Type { get; set; }

        public DateTime Time { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
