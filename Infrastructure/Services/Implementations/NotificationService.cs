using NotifyHub.Infrastructure.Repositories.Implementations;
using NotifyHub.Infrastructure.Repositories.Interfaces;
using NotifyHub.Infrastructure.Services.Interfaces;
using NotifyHub.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotifyHub.Infrastructure.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _notificationRepository.GetAllAsync();
        }

        public Task<Notification> CreateNotificationAsync(Notification notification)
        {
            if (notification == null)
                throw new ArgumentNullException(nameof(notification));

            _notificationRepository.Insert(notification);
            _notificationRepository.SaveAsync();
            return Task.FromResult(notification);
        }

        public Task CreateRecipientsAsync(IEnumerable<NotificationRecipient> recipients)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetNotificationsForCustomer(object id)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}