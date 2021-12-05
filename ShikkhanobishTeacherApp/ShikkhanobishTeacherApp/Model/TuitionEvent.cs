using System;
using System.Collections.Generic;
using System.Text;

namespace ShikkhanobishTeacherApp.Model
{
    public class TuitionEvent
    {
        public EventHandler<CustomEventArgs> clickEvent;

        public void OnCall(CustomEventArgs thisargs)
        {           
            clickEvent.Invoke(this, thisargs);
        }
    }
}
