﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ServicesAPI.Models
{
    public class VISTAResponse
    {
        public HttpResponseMessage Response { get; set; }
        public object Result { get; set; }
    }
}