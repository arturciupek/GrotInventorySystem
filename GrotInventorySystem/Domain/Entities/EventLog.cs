namespace GrotInventorySystem.Domain.Entities;

    public class EventLog
    {
        public Guid Id { get; set; }
        public string Action { get; set; } = default!;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public Guid? UserId { get; set; }
    }

