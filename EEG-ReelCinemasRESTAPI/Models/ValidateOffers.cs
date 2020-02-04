using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class ValidateOffersReq
    {
        public int OfferId { get; set; }
        public string offerHeader { get; set; }
        public string ValidationType { get; set; }
        public string CinemaId { get; set; }
        public string SessionId { get; set; }
        public string BinFirstSet { get; set; }
        public string BinSecondSet { get; set; }
        public string VouchersOrPromos { get; set; }
        public string MemberId { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public string ResultKey { get; set; }
        public string Module { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}