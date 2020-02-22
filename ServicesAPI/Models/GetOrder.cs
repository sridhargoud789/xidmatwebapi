using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class GetOrderReq
    {
        public string UserSessionId { get; set; }
        public string OptionalClientId { get; set; }
        public bool ProcessOrderValue { get; set; }

        public string CinemaId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}