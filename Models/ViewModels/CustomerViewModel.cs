using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using NotifyHub.Models.Domain;

namespace NotifyHub.Models.ViewModels
{
    public class CustomerViewModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;
        [DisplayName("URL of the Service")]
        [DataType(DataType.Url)]
        public string ConnectionDetail { get; set; }
        public virtual ICollection<CustomerEmail> CustomerEmails { get; set; }
    }
}