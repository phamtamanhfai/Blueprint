using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class QuestionItem
    {
        public int ID { get; set; }
        public int STT { get; set; }
        public int IDHeader { get; set; }
        public string Content { get; set; }
        public string Hint { get; set; }
    }
}