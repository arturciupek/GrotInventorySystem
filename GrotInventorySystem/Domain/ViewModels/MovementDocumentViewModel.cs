namespace GrotInventorySystem.Domain.ViewModels;

public class MovementDocumentViewModel
{
    public string DocumentNumber { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? WeaponSerialNumber { get; set; }  
    public string? ModuleName { get; set; }           
    public string? ModuleSerialNumber { get; set; }
    public string? FromLocationName { get; set; }     
    public string? ToLocationName { get; set; }       
    public string? CreatedByEmail { get; set; }
}
