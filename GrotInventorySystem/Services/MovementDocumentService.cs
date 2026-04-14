using GrotInventorySystem.Data;
using GrotInventorySystem.Domain.Entities;
using System.Security.Claims;

namespace GrotInventorySystem.Services
{
    public class MovementDocumentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovementDocumentService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CreateAsync(
            string documentnumber,
            Guid? weaponId,
            Guid? moduleId,
            Guid? fromLocationId,
            Guid? toLocationId)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Guid? userId = Guid.TryParse(userIdString, out var parsedId) ? parsedId : null;

            var move = new MovementDocument
            {
                Id = Guid.NewGuid(),
                DocumentNumber = documentnumber,
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = userId ?? Guid.Empty,
                WeaponId = weaponId,
                ModuleId = moduleId,
                FromLocationId = fromLocationId,
                ToLocationId = toLocationId
            };

            _db.MovementDocuments.Add(move);
            await _db.SaveChangesAsync();
            return true;

        }
    }
}

