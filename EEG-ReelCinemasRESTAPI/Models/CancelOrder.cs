using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class CancelOrderReq
    {
        public string UserSessionId { get; set; }
        public string OptionalClientId { get; set; }
        public string CinemaId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}