using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyHub.Infrastructure.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task CreateRecipientsAsync(IEnumerable<NotificationRecipient> recipients);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetByIdAsync(object id);
        Task<IEnumerable<Notification>> GetNotificationsForCustomer(object id);
    }
}
