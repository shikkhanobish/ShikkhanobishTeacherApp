using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishTeacherApp.Model
{
    public class CustomEventArgs: EventArgs
    {
        public string tuitionLogID { get; set; }
        public string name { get; set; }
    }
}
