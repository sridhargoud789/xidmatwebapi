using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class EnrollUByEmaarMember
    {
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string DeviceType { get; set; }

        public string MemberId { get; set; }
        public string MobileBookingId { get; set; }
        public int bookingInfoId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}