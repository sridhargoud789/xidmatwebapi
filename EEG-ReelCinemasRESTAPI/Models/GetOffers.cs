using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class GetOffersReq
    {

      
            public string CinemaId { get; set; }
            public string SessionId { get; set; }
            public string Experience { get; set; }
            public int DataFrom { get; set; }

        public DeviceDetails DeviceDetails { get; set; }
    }
}