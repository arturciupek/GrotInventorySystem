namespace GrotInventorySystem.Domain.Entities;

public class WeaponModuleAssignment
{
    public Guid Id { get; set; }

    public Guid WeaponId { get; set; }
    public Weapon Weapon { get; set; } = default!;

    public Guid ModuleId { get; set; }
    public Module Module { get; set; } = default!;

    public DateTime MountedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UnmountedAtUtc { get; set; }
}