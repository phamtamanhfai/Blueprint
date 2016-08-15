using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}