using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginMVC.Models
{
    public class UserViewModel
    {
        public IEnumerable<SelectListItem> UserRoleList { get; set; }  
        public string Id { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
       // public string PasswordHash { get; set; }
        //public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }
        
    }
}