using NotifyHub.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NotifyHub.Models.ViewModels
{
    public class NotificationViewModel
    {
        [Required, MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(500)]
        public string ShortDescription { get; set; }

        [Required]
        public string LongDescription { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        public string ImageUrl { get; set; }

        public NotificationStatus Status { get; set; }

        public NotificationPriority Priority { get; set; }
    }
}