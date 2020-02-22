using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServicesAPI.Models.CompleteOrderByCard;
using ReelDVO;

namespace ServicesAPI.Models
{
    public class CompleteOrderByCardReq
    {
        public string UserSessionId { get; set; }
        public PaymentInfo PaymentInfo { get; set; }

        public bool PerformPayment { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }

        public string OptionalClientId { get; set; }

        public string BookingNotes { get; set; }

        public string SessionId { get; set; }

        public string CinemaId { get; set; }

        public string DeviceType { get; set; }

        public bool isNewsletter { get; set; }
        public bool isIntrestedForLuckyDraw { get; set; }
        public int UserId { get; set; }
        public DeviceDetails DeviceDetails { get; set; }
    }
}