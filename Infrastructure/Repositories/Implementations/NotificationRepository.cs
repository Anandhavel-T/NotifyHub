using NotifyHub.Data;
using NotifyHub.Infrastructure.Repositories.Interfaces;
using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotifyHub.Infrastructure.Repositories.Implementations
{
	public class NotificationRepository : BaseRepository<Notification> ,INotificationRepository
	{
        public NotificationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _dbSet 
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _dbSet
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
