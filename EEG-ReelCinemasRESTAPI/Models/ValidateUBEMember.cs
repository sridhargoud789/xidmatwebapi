using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class ValidateUBEMemberReq
    {
        public string UBEMemberId { get; set; }
        public string UBEMemberPassword { get; set; }
        public string DeviceType { get; set; }
        public int TicketPriceInCents { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }

    public class GetUBEPointsByTicketPrice
    {
        public int TicketPriceInCents { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}