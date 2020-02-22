using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.CompleteOrderByUBELoyalty
{
    public class PaymentInfo
    {
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string PaymentTenderCategory { get; set; }
        public int PaymentValueCents { get; set; }
        public string PaymentSystemId { get; set; }
    }
}
namespace ServicesAPI.Models.CompleteOrderByCard
{
    public class PaymentInfo
    {
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public int PaymentValueCents { get; set; }
        public string PaymentSystemId { get; set; }
        public string CardCVC { get; set; }
        public string tokenIdentifier { get; set; }
        public int bookingInfoId { get; set; }
        public string UserSessionId { get; set; }
        public string CinemaId { get; set; }
        public string SessionId { get; set; }
        public string DeviceType { get; set; }
    }
}

namespace ServicesAPI.Models.CompleteOrderByGiftCard
{
    public class PaymentInfo
    {
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public int PaymentValueCents { get; set; }
        public string PaymentTenderCategory { get; set; }
        public string PaymentSystemId { get; set; }
        public bool BillFullOutstandingAmount { get; set; }
        public bool UseAsBookingRef { get; set; }
        public int BillingValueCents { get; set; }
        //public int CardBalance { get; set; }
        public bool SaveCardToWallet { get; set; }
    }
}