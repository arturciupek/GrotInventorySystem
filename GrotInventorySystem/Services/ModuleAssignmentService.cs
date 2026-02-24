using Microsoft.EntityFrameworkCore;
using GrotInventorySystem.Data;
using GrotInventorySystem.Domain.Entities;

namespace GrotInventorySystem.Services;

public class ModuleAssignmentService
{
    private readonly ApplicationDbContext _db;

    public ModuleAssignmentService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> MountModuleAsync(Guid moduleId, Guid weaponId)
    {
        // 1) Czy moduł już jest aktywnie zamontowany?
        var activeAssignment = await _db.WeaponModuleAssignments
            .FirstOrDefaultAsync(x => x.ModuleId == moduleId && x.UnmountedAtUtc == null);

        if (activeAssignment != null)
        {
            return false; // moduł już jest zamontowany
        }

        // 2) Pobierz moduł
        var module = await _db.Modules.FirstOrDefaultAsync(x => x.Id == moduleId);
        if (module == null)
        {
            return false;
        }

        // 3) (Opcjonalnie) sprawdź czy broń istnieje
        var weaponExists = await _db.Weapons.AnyAsync(x => x.Id == weaponId);
        if (!weaponExists)
        {
            return false;
        }

        // 4) Moduł nie jest już w lokalizacji (bo jest montowany na broni)
        module.LocationId = null;

        // 5) Dodaj wpis historii montażu
        var assignment = new WeaponModuleAssignment
        {
            Id = Guid.NewGuid(),
            ModuleId = moduleId,
            WeaponId = weaponId,
            MountedAtUtc = DateTime.UtcNow,
            UnmountedAtUtc = null
        };

        _db.WeaponModuleAssignments.Add(assignment);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UnmountModuleAsync(Guid moduleId, Guid targetLocationId)
    {
        // 1) Znajdź aktywny montaż
        var activeAssignment = await _db.WeaponModuleAssignments
            .FirstOrDefaultAsync(x => x.ModuleId == moduleId && x.UnmountedAtUtc == null);

        if (activeAssignment == null)
        {
            return false; // moduł nie jest aktualnie zamontowany
        }

        // 2) Pobierz moduł
        var module = await _db.Modules.FirstOrDefaultAsync(x => x.Id == moduleId);
        if (module == null)
        {
            return false;
        }

        // 3) Sprawdź czy lokalizacja istnieje
        var locationExists = await _db.Locations.AnyAsync(x => x.Id == targetLocationId);
        if (!locationExists)
        {
            return false;
        }

        // 4) Zamknij montaż (historia)
        activeAssignment.UnmountedAtUtc = DateTime.UtcNow;

        // 5) Moduł wraca do lokalizacji
        module.LocationId = targetLocationId;

        await _db.SaveChangesAsync();
        return true;
    }
}