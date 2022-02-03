using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishTeacherApp.Model
{
    public class Topic
    {
        public int topicID { get; set; }
        public int chapterID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int purchaseRate { get; set; }
    }
}
