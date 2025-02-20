using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace NotifyHub.Models.Domain
{
    [Table("customers")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;  // Active/inactive customers

        [MaxLength(20)]
        public string Phone { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }

        [ForeignKey("UpdatedById")]
        public virtual User UpdatedBy { get; set; }

        // Navigation properties
        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
        public virtual ICollection<CustomerEmail> CustomerEmails { get; set; }
    }
}