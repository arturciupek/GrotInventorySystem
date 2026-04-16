namespace GrotInventorySystem.Domain.ViewModels;

public class MovementDocumentViewModel
{
    public string DocumentNumber { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string? WeaponSerialNumber { get; set; }  // numer seryjny broni
    public string? ModuleName { get; set; }           // nazwa modułu
    public string? FromLocationName { get; set; }     // nazwa lokalizacji skąd
    public string? ToLocationName { get; set; }       // nazwa lokalizacji dokąd
    public string? CreatedByEmail { get; set; }
}
