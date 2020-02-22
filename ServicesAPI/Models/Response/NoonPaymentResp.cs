using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesAPI.Models.Response
{
   

    public class NoonPaymentResp
    {
        public int resultCode { get; set; }
        public string message { get; set; }
        public int resultClass { get; set; }
        public string classDescription { get; set; }
        public string actionHint { get; set; }
        public string requestReference { get; set; }
        public NPRResult result { get; set; }
    }

    public class NPRResult
    {
        public string nextActions { get; set; }
        public NPROrder order { get; set; }
        public NPRConfiguration configuration { get; set; }
        public NPRBusiness business { get; set; }
        public NPRCheckoutdata checkoutData { get; set; }
        public NPRFraudcheck fraudCheck { get; set; }
        public NPRAuthentication authentication { get; set; }
        public NPRDevicefingerprint deviceFingerPrint { get; set; }
        public NPRPayerauthentication payerAuthentication { get; set; }
        public NPRPaymentdetails paymentDetails { get; set; }
    }

    public class NPROrder
    {
        public string status { get; set; }
        public DateTime creationTime { get; set; }
        public long id { get; set; }
        public float amount { get; set; }
        public string currency { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string channel { get; set; }
    }

    public class NPRConfiguration
    {
        public bool tokenizeCc { get; set; }
        public string returnUrl { get; set; }
        public string locale { get; set; }
        public DateTime initiationValidity { get; set; }
        public string paymentAction { get; set; }
    }

    public class NPRBusiness
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class NPRCheckoutdata
    {
        public string postUrl { get; set; }
        public string jsUrl { get; set; }
    }

    public class NPRFraudcheck
    {
        public NPRInternal _internal { get; set; }
    }

    public class NPRInternal
    {
        public string result { get; set; }
        public string screeningResponse { get; set; }
    }

    public class NPRAuthentication
    {
        public NPRRequest request { get; set; }
    }

    public class NPRRequest
    {
        public string url { get; set; }
        public string method { get; set; }
        public NPRNvp nvp { get; set; }
    }

    public class NPRNvp
    {
        public string MD { get; set; }
        public string PaReq { get; set; }
        public string TermUrl { get; set; }
    }

    public class NPRDevicefingerprint
    {
        public string sessionId { get; set; }
    }

    public class NPRPayerauthentication
    {
        public NPREnrollment enrollment { get; set; }
    }

    public class NPREnrollment
    {
        public string status { get; set; }
        public string statusDescription { get; set; }
        public string xid { get; set; }
    }

    public class NPRPaymentdetails
    {
        public string tokenIdentifier { get; set; }
        public string instrument { get; set; }
        public string mode { get; set; }
        public string integratorAccount { get; set; }
        public string paymentInfo { get; set; }
        public string paymentMechanism { get; set; }
        public string brand { get; set; }
        public string scheme { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string isNetworkToken { get; set; }
        public string cardType { get; set; }
        public string cardCountry { get; set; }
        public string cardCountryName { get; set; }
    }

}