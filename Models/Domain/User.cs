using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace NotifyHub.Models.Domain
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(50), Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }  // Hashed password

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime")]
        public DateTime? LastLoginDate { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        // Audit fields
        public int? CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        // Navigation properties
        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }

        [ForeignKey("UpdatedById")]
        public virtual User UpdatedBy { get; set; }
    }
}