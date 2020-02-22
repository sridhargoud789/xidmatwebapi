using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicesAPI.Models.CompleteOrderByUBELoyalty;
using ReelDVO;

namespace ServicesAPI.Models
{
    public class CompleteOrderByUBELoyaltyReq
    {
        public TransactionDetailsDvo TransactionDetailsDvo { get; set; }
        public string SecurityToken { get; set; }
        public string RewardTypeExternalReference { get; set; }

        public string LocationExternalReference { get; set; }
        public double DollarAmountToRedeem { get; set; }
        public double LoyaltyPoints { get; set; }
        public string ApplicationDownloadedDate { get; set; }
        public Int64 bookingInfoId { get; set; }
        public string OptionalClientId { get; set; }

        public DeviceDetails DeviceDetails { get; set; }
    }
}
