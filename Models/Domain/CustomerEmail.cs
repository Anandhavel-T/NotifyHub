using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NotifyHub.Models.Domain
{
    [Table("customerEmails")]
    public class CustomerEmail
    {
        [Key]
        public int Id { get; set; }

        [Index]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [Index("IX_CustomerEmail_Email", IsUnique = false)]
        public string Email { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}