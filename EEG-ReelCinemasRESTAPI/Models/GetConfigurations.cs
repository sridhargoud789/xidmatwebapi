﻿using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class GetConfigurationsReq
    {
        public string DeviceOS { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}