using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class GetConcessionListReq
    {
        public string CinemaId { get; set; }
        public string SessionId { get; set; }
        public string DeviceType { get; set; }
        public string TicketTypeCode { get; set; }

        public string ClientId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}
