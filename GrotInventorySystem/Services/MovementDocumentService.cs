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

            await _db.SaveChangesAsync();

            var fromLocation = await _db.Locations.FindAsync(fromLocationId);
            var toLocation = await _db.Locations.FindAsync(toLocationId);

            await _eventLogService.LogAsync(
                $"Utworzono dokument ruchu {documentNumber} (z: {fromLocation?.Name}, do: {toLocation?.Name})");

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