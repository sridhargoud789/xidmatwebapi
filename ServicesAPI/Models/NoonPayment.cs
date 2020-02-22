using ReelDVO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models
{
    public class NoonPayment
    {
    }

    public class InitiateCardPaymentReq
    {
        public int UserId { get; set; }
        public string CustomerEmail { get; set; }
        
        public string CustomerPhone { get; set; }
        public string PhoneCountryCode { get; set; }
        public string CustomerName { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public string PaymentValueCents { get; set; }
        public bool IsSaveCard { get; set; }
        public string PaymentSystemId { get; set; }
        public string CardCVC { get; set; }
        public string tokenIdentifier { get; set; }
        public bool isNewsLetter { get; set; }
        public bool isIntrestedForLuckyDraw { get; set; }
        public int bookingInfoId { get; set; }
        public string UserSessionId { get; set; }
        public string OptionalClientId { get; set; }
        public string CinemaId { get; set; }
        public string SessionId { get; set; }
        public string DeviceType { get; set; }
        public DeviceDetails DeviceDetails { get; set; }

    }

    public class InitiateCardPayment
    {
        public string apiOperation { get; set; }
        public InitiateCardPaymentOrder order { get; set; }
        public InitiateCardPaymentConfiguration configuration { get; set; }
        public InitiateCardPaymentdata paymentData { get; set; }
    }

    public class InitiateCardPaymentOrder
    {
        public string name { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string channel { get; set; }
        public string category { get; set; }
        public string description { get; set; }
    }

    public class InitiateCardPaymentConfiguration
    {
        public string returnUrl { get; set; }
        public string locale { get; set; }
        public bool tokenizeCc { get; set; }
        public string initiationValidity { get; set; }
        public string paymentAction { get; set; }
    }

    public class InitiateCardPaymentdata
    {
        public string type { get; set; }
        public InitiateCardPaymentCardData data { get; set; }
    }

    public class InitiateCardPaymentCardData
    {
        public string numberPlain { get; set; }
        public string cvv { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
    }


    public class InitiatePaymentByToken
    {
        public string apiOperation { get; set; }
        public PaymentByTokenOrder order { get; set; }
        public PaymentByTokenConfiguration configuration { get; set; }
        public PaymentByTokenPaymentdata paymentData { get; set; }
    }

    public class PaymentByTokenOrder
    {
        public string name { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string channel { get; set; }
        public string category { get; set; }
        public string description { get; set; }
    }

    public class PaymentByTokenConfiguration
    {
        public string returnUrl { get; set; }
        public string locale { get; set; }
        public bool tokenizeCc { get; set; }
        public string initiationValidity { get; set; }
        public string paymentAction { get; set; }
    }

    public class PaymentByTokenPaymentdata
    {
        public string type { get; set; }
        public PaymentByTokenData data { get; set; }
    }

    public class PaymentByTokenData
    {
        public string cvv { get; set; }
        public string tokenIdentifier { get; set; }
    }


}