using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class ChangePassItem
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}