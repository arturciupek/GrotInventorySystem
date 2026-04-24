namespace GrotInventorySystem.Domain.ViewModels;

public class ReportViewModel
{
    public string LocationName { get; set; } = default!;
    public int WeaponCount {  get; set; }
    public int ModuleCount { get; set; }
}
