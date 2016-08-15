using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace Blueprint.Models
{
    public class FeedbackItem
    {
        public int ID { get; set; }
        public string StudentName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string Career { get; set; }
        public string Quote { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
    }
}