using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace NotifyHub.Models.Domain
{
    [Table("notification_recipients")]
    public class NotificationRecipient
    {
        [Key]
        public int Id { get; set; }

        [Index]
        public int CustomerId { get; set; }

        [Index]
        public int NotificationId { get; set; }

        public bool IsRead { get; set; } = false;
        public bool IsDelivered { get; set; } = false;

        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime")]
        public DateTime? ReadAt { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime")]
        public DateTime? DeliveredAt { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }
    }
}