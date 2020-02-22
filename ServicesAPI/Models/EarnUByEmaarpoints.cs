using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class EarnUByEmaarpoints
    {
        public string MemberId { get; set; }
        //public string MobileBookingId { get; set; }

        //public string MobileNo { get; set; }

        //public string Name { get; set; }

        //public string Email { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
    public class UbyEmaarConfig
    {
        public string CinemaId { get; set; }
        public string ClientProfileId { get; set; }
    }

}