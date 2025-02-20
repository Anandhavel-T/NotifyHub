using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotifyHub.Models.Domain
{
    [Table("notifications")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        [MaxLength(500)]
        public string ShortDescription { get; set; }

        [Required]
        public string LongDescription { get; set; }

        [Required]
        public NotificationType Type { get; set; }  // Enum for Notification Types

        public string ImageUrl { get; set; }  // Store image path instead of raw data

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime ScheduledAt { get; set; } = DateTime.UtcNow.AddMinutes(1);

        [DataType(DataType.DateTime)]
        public DateTime? ExpiryAt { get; set; }

        public NotificationStatus Status { get; set; } = NotificationStatus.Draft;

        public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

        public bool IsGlobal { get; set; } = true;  // If true, send to all customers

        // Audit fields
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        // Navigation properties
        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }

        [ForeignKey("UpdatedById")]
        public virtual User UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedById { get; set; }

        public virtual ICollection<NotificationRecipient> Recipients { get; set; }
    }

    public enum NotificationStatus
    {
        Draft,
        Scheduled,
        Sent,
        Cancelled,
        Expired
    }

    public enum NotificationPriority
    {
        Low,
        Normal,
        High,
        Urgent
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Alert
    }
}