namespace GrotInventorySystem.Domain.ViewModels
{
    public class EventLogViewModel
    {
        public DateTime Created { get; set; }
        public string Action { get; set; } = default!;
        public string? UserEmail { get; set; }
    }
}
