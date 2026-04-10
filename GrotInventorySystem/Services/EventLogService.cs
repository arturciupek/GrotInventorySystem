using GrotInventorySystem.Data;
using GrotInventorySystem.Domain.Entities;

namespace GrotInventorySystem.Services
{
    public class EventLogService
    {
        private readonly ApplicationDbContext _db;

        public EventLogService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> LogAsync(string action)
        {
            var log = new EventLog
            {
                Id = Guid.NewGuid(),
                Action = action,
                Created = DateTime.UtcNow,
                UserId = null
            };

            _db.EventLogs.Add(log);

            await _db.SaveChangesAsync();
            return true;

        }
    }
}
