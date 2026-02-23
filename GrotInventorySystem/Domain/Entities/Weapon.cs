namespace GrotInventorySystem.Domain.Entities;

public class Weapon
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; } = default!;

    public Guid LocationId { get; set; }
    public Location Location { get; set; } = default!;
}