using NotifyHub.Infrastructure.Repositories.Implementations;
using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyHub.Infrastructure.Repositories.Interfaces
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetAllAsync();

        Task<Notification> GetByIdAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
