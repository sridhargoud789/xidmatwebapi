using EEG_ReelCinemasRESTAPI.Models.CompleteOrderByUBELoyalty;
using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EEG_ReelCinemasRESTAPI.Models
{
    public class CompleteOrderByGiftCardReq
    {
        public TransactionDetailsDvoForGift TransactionDetailsDvoForGift { get; set; }

        public string GiftCardNumber { get; set; }
        public Int64 bookingInfoId { get; set; }
        public string ApplicationDownloadedDate { get; set; }
        public DeviceDetails DeviceDetails { get; set; }

        //public string UserSessionId { get; set; }
        //public PaymentInfo PaymentInfo { get; set; }

        //public bool PerformPayment { get; set; }
        //public string CustomerEmail { get; set; }
        //public string CustomerPhone { get; set; }
        //public string CustomerName { get; set; }

        //public string OptionalClientId { get; set; }

        //public string BookingNotes { get; set; }
        //public string SessionId { get; set; }
        //public string CinemaId { get; set; }
        //public string DeviceType { get; set; }
    }
}