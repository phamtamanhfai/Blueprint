using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class TestHeader
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int NoQuestions { get; set; }
        public int ActualNoQuestions { get; set; }
        public string Status { get; set; }
    }
}