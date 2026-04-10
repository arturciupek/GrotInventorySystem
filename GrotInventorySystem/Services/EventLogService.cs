using GrotInventorySystem.Data;
using GrotInventorySystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GrotInventorySystem.Services
{
    public class EventLogService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventLogService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LogAsync(string action)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Guid? userId = Guid.TryParse(userIdString, out var parsedId) ? parsedId : null;
            var log = new EventLog
            {
                Id = Guid.NewGuid(),
                Action = action,
                Created = DateTime.UtcNow,
                UserId = userId
            };

            _db.EventLogs.Add(log);
            await _db.SaveChangesAsync();
            return true;

        }
    }
}
