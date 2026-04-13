namespace GrotInventorySystem.Domain.Entities;

public class MovementDocument
{
    public Guid Id { get; set; }
    public string DocumentNumber { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedByUserId { get; set; }
    public Guid? WeaponId { get; set; }
    public Guid? ModuleId { get; set; }
    public Guid? FromLocationId { get; set; }
    public Guid? ToLocationId { get; set;}
}
