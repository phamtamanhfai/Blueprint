using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class CoursesItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Duration { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
    }
}