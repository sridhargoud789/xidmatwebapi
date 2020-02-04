using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class CompleteOrderLoyaltyCardReq
    {
        public TransactionDetailsDvo TransactionDetailsDvo { get; set; }
        public string SecurityToken { get; set; }
        public string RewardTypeExternalReference { get; set; }
        public string LocationExternalReference { get; set; }
        public double DollarAmountToRedeem { get; set; }
        public double LoyaltyPoints { get; set; }
        public string ApplicationDownloadedDate { get; set; }
    }
}