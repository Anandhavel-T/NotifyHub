using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NotifyHub.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(255)]
        [DisplayName("URL of the Service")]
        [DataType(DataType.Url)]
        public string ConnectionDetail { get; set; }

        public List<CustomerEmailViewModel> CustomerEmails { get; set; } = new List<CustomerEmailViewModel>();
    }

    public class CustomerEmailViewModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; } = true;
    }
}