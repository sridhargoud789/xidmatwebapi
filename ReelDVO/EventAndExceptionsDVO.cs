using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReelDvo
{
    public class EventAndExceptionsDVO
    {
        public EventAndExceptionsDVO()
        {
            Mode = "event";
        }

        public string Session { get; set; }
        public string Mode { get; set; }
        public string Device { get; set; }
        public string AppType { get; set; }
        public string Record { get; set; }
    }
}
