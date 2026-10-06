using GrotInventorySystem.Data;
using GrotInventorySystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GrotInventorySystem.Services
{
    public class MovementDocumentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EventLogService _eventLogService;

        public MovementDocumentService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, EventLogService eventLogService)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _eventLogService = eventLogService;
        }

        public async Task<string> CreateAsync(
            Guid? weaponId,
            Guid? moduleId,
            Guid? fromLocationId,
            Guid? toLocationId)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid? userId = Guid.TryParse(userIdString, out var parsedId) ? parsedId : null;

            var documentNumber = await GenerateDocumentNumberAsync();

            // Sprawdzanie czy moduł nie jest zamontowany
            if (moduleId.HasValue)
            {
                var isMounted = await _db.WeaponModuleAssignments
                    .AnyAsync(x => x.ModuleId == moduleId.Value && x.UnmountedAtUtc == null);
                if (isMounted)
                    return "BŁĄD: Moduł jest zamontowany na broni!";
            }

            var move = new MovementDocument
            {
                Id = Guid.NewGuid(),
                DocumentNumber = documentNumber,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId ?? Guid.Empty,
                WeaponId = weaponId,
                ModuleId = moduleId,
                FromLocationId = fromLocationId,
                ToLocationId = toLocationId
            };

            _db.MovementDocuments.Add(move);

            // Zmiana lokalizacji broni i zamontowanych modułów
            if (weaponId.HasValue && toLocationId.HasValue)
            {
                var weapon = await _db.Weapons.FindAsync(weaponId.Value);
                if (weapon != null)
                {
                    weapon.LocationId = toLocationId.Value;

                    var mountedModules = await _db.WeaponModuleAssignments
                        .Where(x => x.WeaponId == weaponId.Value && x.UnmountedAtUtc == null)
                        .Include(x => x.Module)
                        .ToListAsync();

                    foreach (var assignment in mountedModules)
                    {
                        assignment.Module.LocationId = toLocationId.Value;
                    }
                }
            }

            // Zmiana lokalizacji modułu
            if (moduleId.HasValue && toLocationId.HasValue)
            {
                var module = await _db.Modules.FindAsync(moduleId.Value);
                if (module != null)
                    module.LocationId = toLocationId.Value;
            }

            await _db.SaveChangesAsync();

            var fromLocation = await _db.Locations.FindAsync(fromLocationId);
            var toLocation = await _db.Locations.FindAsync(toLocationId);

            if (moduleId.HasValue)
            {
                var module = await _db.Modules.FindAsync(moduleId.Value);
                await _eventLogService.LogAsync(
                    $"Przesunięto moduł {module?.SerialNumber} z {fromLocation?.Name} do {toLocation?.Name} (dok. {documentNumber})");
            }
            else if (weaponId.HasValue)
            {
                var weapon = await _db.Weapons.FindAsync(weaponId.Value);
                await _eventLogService.LogAsync(
                    $"Przesunięto broń {weapon?.SerialNumber} z {fromLocation?.Name} do {toLocation?.Name} (dok. {documentNumber})");
            }

            return documentNumber;
        }

        private async Task<string> GenerateDocumentNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"DR-{year}-";

            var lastNumber = await _db.MovementDocuments
                .Where(d => d.DocumentNumber.StartsWith(prefix))
                .OrderByDescending(d => d.DocumentNumber)
                .Select(d => d.DocumentNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastNumber != null)
            {
                var numberPart = lastNumber.Substring(prefix.Length);
                if (int.TryParse(numberPart, out var parsed))
                    nextNumber = parsed + 1;
            }

            return $"{prefix}{nextNumber:D4}";
        }
    }
}