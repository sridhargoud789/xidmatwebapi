using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class GetBookingInfoReq
    {
        public string BookingId { get; set; }
        public string BookingNumber { get; set; }
        public string CinemaId { get; set; }
        public string OptionalClientId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
        //public string UserSessionId { get; set; }
        //public string SessionId { get; set; }
        //public string DeviceType { get; set; }
    }
}