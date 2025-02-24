using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifyHub.Models.ViewModels
{
    public class UserViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Id { get; internal set; }
    }
}