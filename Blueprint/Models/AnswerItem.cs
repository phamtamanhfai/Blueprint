using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blueprint.Models
{
    public class AnswerItem
    {
        public int ID { get; set; }
        public int IDQuestion { get; set; }
        public string Content { get; set; }
        public bool IsCorrect{ get; set; }
    }
}