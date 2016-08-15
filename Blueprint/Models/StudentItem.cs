using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class StudentItem
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Purpose { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public int? UserID { get; set; }

    }
}