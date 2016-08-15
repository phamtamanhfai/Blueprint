using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class UserInfoItem
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsOwner { get; set; }
        public string Status { get; set; }
    }
}