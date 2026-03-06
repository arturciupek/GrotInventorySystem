using GrotInventorySystem.Domain.Enums;

namespace GrotInventorySystem.Domain.Entities;

public class Module
{
    public Guid Id { get; set; }
    public string SerialNumber { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }
    public ModuleStatus Status { get; set; } = ModuleStatus.Active;
}