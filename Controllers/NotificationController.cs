using NotifyHub.Infrastructure.Services.Implementations;
using NotifyHub.Infrastructure.Services.Interfaces;
using NotifyHub.Models.Domain;
using NotifyHub.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NotifyHub.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly ICustomerService _customerService;

        public NotificationController(INotificationService notificationService, ICustomerService customerService)
        {
            _notificationService = notificationService;
            _customerService = customerService;
        }

        public async Task<ActionResult> Index()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return View(notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check the form and try again.";
                return RedirectToAction("Index");
            }

            try
            {
                var notification = new Notification
                {
                    Subject = model.Subject,
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.LongDescription,
                    Type = model.Type,
                    ImageUrl = model.ImageUrl,
                    Status = model.Status,
                    Priority = model.Priority,
                    CreatedAt = DateTime.UtcNow,
                    //CreatedById = User.Identity.GetUserId<int>()
                };

                await _notificationService.CreateNotificationAsync(notification);

                if (model.Status == NotificationStatus.Sent)
                {
                    await CreateNotificationRecipients(notification);
                }

                TempData["Success"] = "Notification created successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        private async Task CreateNotificationRecipients(Notification notification)
        {
            var customers = notification.IsGlobal
                ? await _customerService.GetAllCustomersAsync()
                : await _customerService.GetActiveCustomersAsync();

            var recipients = customers.Select(c => new NotificationRecipient
            {
                CustomerId = c.Id,
                NotificationId = notification.Id,
                IsRead = false,
                IsDelivered = false
            });

            await _notificationService.CreateRecipientsAsync(recipients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(NotificationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please check the form and try again.";
                return RedirectToAction("Index");
            }

            try
            {
                var notification = await _notificationService.GetByIdAsync(model.Id);
                if (notification == null)
                {
                    TempData["Error"] = "Notification not found.";
                    return RedirectToAction("Index");
                }

                // Update notification properties
                notification.Subject = model.Subject;
                notification.ShortDescription = model.ShortDescription;
                notification.LongDescription = model.LongDescription;
                notification.Type = model.Type;
                notification.ImageUrl = model.ImageUrl;
                notification.Status = model.Status;
                notification.Priority = model.Priority;
                //notification.UpdatedAt = DateTime.UtcNow;
                //notification.UpdatedById = User.Identity.GetUserId<int>();

                // Handle scheduling
                //if (model.Status == NotificationStatus.Scheduled &&
                //    model.ScheduledAt > DateTime.UtcNow)
                //{
                //    notification.ScheduledAt = model.ScheduledAt;
                //}

                //// Handle expiry
                //if (model.ExpiryAt.HasValue && model.ExpiryAt.Value > DateTime.UtcNow)
                //{
                //    notification.ExpiryAt = model.ExpiryAt;
                //}

                //await _notificationService.UpdateNotificationAsync(notification);
                //TempData["Success"] = "Notification updated successfully.";

                // If status changed to Sent, create notification recipients
                if (model.Status == NotificationStatus.Sent &&
                    notification.Status != NotificationStatus.Sent)
                {
                    await CreateNotificationRecipients(notification);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}