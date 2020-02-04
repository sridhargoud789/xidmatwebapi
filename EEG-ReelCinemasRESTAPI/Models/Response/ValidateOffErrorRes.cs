using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models.Response
{
    public class ValidateOffErrorRes
    {   
        public int statusCode { get; set; }
        public string statusMessage { get; set; }
        public object Experience { get; set; }
        public string Sourcedata { get; set; }
        public int BookingTimer { get; set; }
        public int AddExtraTimer { get; set; }
        public int WhenToShowAddTimer { get; set; }
    }

}