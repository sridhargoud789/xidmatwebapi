using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class InfluxResponse
    {
        public HttpResponseMessage Response { get; set; }
        public object Result { get; set; }
    }
}